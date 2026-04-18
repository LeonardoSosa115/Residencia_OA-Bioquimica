using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectModuleController : MonoBehaviour
{
    private const string SIMULATOR_SCENE = "FQSimulator";
    private const string LOGIN_SCENE     = "Login";

    public void OnFisicoquimicaClicked()
    {
        SceneManager.LoadScene(SIMULATOR_SCENE);
    }

    public void OnBiocineticaClicked()
    {
        
        Debug.Log("Biocinetica: aún no implementado");
    }

    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene(LOGIN_SCENE);
    }
}