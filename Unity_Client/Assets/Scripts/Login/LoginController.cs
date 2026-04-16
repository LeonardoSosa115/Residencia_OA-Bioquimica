using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginController : MonoBehaviour
{
    [Header("Campos")]
    public TMP_InputField inputEmail;
    public TMP_InputField inputPassword;

    [Header("Feedback")]
    public TextMeshProUGUI txtError;

    private const string SELECT_MODULE_SCENE = "SelectModule";
    private const string MAIN_MENU_SCENE     = "MainMenu";

    void Start()
    {
        if (txtError != null)
            txtError.gameObject.SetActive(false);
    }

    public void OnIngresarButtonClicked()
    {
        
        if (string.IsNullOrEmpty(inputEmail.text) || 
            string.IsNullOrEmpty(inputPassword.text))
        {
            MostrarError("Completa todos los campos.");
            return;
        }
        // Aqui va la logica del login, por ahora solo valida que los campos esten vacios
        SceneManager.LoadScene(SELECT_MODULE_SCENE);
    }

    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene(MAIN_MENU_SCENE);
    }

    private void MostrarError(string mensaje)
    {
        if (txtError != null)
        {
            txtError.text = mensaje;
            txtError.gameObject.SetActive(true);
        }
    }
}