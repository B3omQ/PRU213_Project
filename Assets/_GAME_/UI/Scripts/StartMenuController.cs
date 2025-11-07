using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{

    SaveController _SaveController;
    public void OnStartClick()
    {
        SceneManager.LoadScene("Home");
    }

    public void OnLoadClick()
    {
        SceneManager.LoadScene("Home");
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
