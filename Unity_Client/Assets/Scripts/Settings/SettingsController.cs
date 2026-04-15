using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour
{
    [Header("Sliders de volumen")]
    public Slider sliderVolumenGeneral;
    public Slider sliderVolumenEfectos;
    public Slider sliderVolumenMusica;

    [Header("Pantalla completa")]
    public Toggle togglePantallaCompleta;

    [Header("Audio")]
    public AudioSource musicAudioSource;

    private const string MAIN_MENU_SCENE = "MainMenu";
    private const string KEY_VOL_GENERAL = "VolGeneral";
    private const string KEY_VOL_EFECTOS = "VolEfectos";
    private const string KEY_VOL_MUSICA  = "VolMusica";
    private const string KEY_FULLSCREEN  = "Fullscreen";

    void Start()
    {
        // Busca el AudioSource automáticamente si no está asignado
        if (musicAudioSource == null)
        {
            musicAudioSource = FindObjectOfType<AudioSource>();
        }

        CargarAjustes();

        sliderVolumenGeneral.onValueChanged.AddListener(v => GuardarAjustes());
        sliderVolumenEfectos.onValueChanged.AddListener(v => GuardarAjustes());
        sliderVolumenMusica.onValueChanged.AddListener(v  => GuardarAjustes());
        togglePantallaCompleta.onValueChanged.AddListener(v => GuardarAjustes());
    }

    void CargarAjustes()
    {
        sliderVolumenGeneral.value  = PlayerPrefs.GetFloat(KEY_VOL_GENERAL, 0.75f);
        sliderVolumenEfectos.value  = PlayerPrefs.GetFloat(KEY_VOL_EFECTOS, 0.75f);
        sliderVolumenMusica.value   = PlayerPrefs.GetFloat(KEY_VOL_MUSICA,  0.75f);
        togglePantallaCompleta.isOn = PlayerPrefs.GetInt(KEY_FULLSCREEN, 0) == 1;

        AplicarAjustes();
    }

    void GuardarAjustes()
    {
        PlayerPrefs.SetFloat(KEY_VOL_GENERAL, sliderVolumenGeneral.value);
        PlayerPrefs.SetFloat(KEY_VOL_EFECTOS, sliderVolumenEfectos.value);
        PlayerPrefs.SetFloat(KEY_VOL_MUSICA,  sliderVolumenMusica.value);
        PlayerPrefs.SetInt(KEY_FULLSCREEN, togglePantallaCompleta.isOn ? 1 : 0);
        PlayerPrefs.Save();

        AplicarAjustes();
    }

    void AplicarAjustes()
    {
        AudioListener.volume = sliderVolumenGeneral.value;
        Screen.fullScreen    = togglePantallaCompleta.isOn;

        if (musicAudioSource != null)
        {
            musicAudioSource.volume = sliderVolumenMusica.value;
        }
    }

    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene(MAIN_MENU_SCENE);
    }
}