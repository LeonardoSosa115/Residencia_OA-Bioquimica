using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Nombre exacto de la escena del simulador
    private const string SIMULATOR_SCENE = "Simulator";

    public void OnBeginButtonClicked()
    {
        SceneManager.LoadScene(SIMULATOR_SCENE);
    }

    public void OnSettingsButtonClicked()
    {
        
        Debug.Log("Ajustes: aún no implementado");
    }

    public void OnExitButtonClicked()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}