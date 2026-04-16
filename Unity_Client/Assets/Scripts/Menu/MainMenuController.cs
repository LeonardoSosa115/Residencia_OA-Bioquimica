using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private const string LOGIN_SCENE    = "Login";
    private const string SETTINGS_SCENE = "Settings";

    public void OnBeginButtonClicked()
    {
        SceneManager.LoadScene(LOGIN_SCENE);
    }

    public void OnSettingsButtonClicked()
    {
        SceneManager.LoadScene(SETTINGS_SCENE);
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