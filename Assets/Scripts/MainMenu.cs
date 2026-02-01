using UnityEngine;
using UnityEngine.SceneManagement;

    //SceneManager.LoadSceneAscync(sceneToLoad, LoadSceneMode.Single);
    //Application.Quit();

public class MainMenu : MonoBehaviour
{
    public string sceneToLoad;

    public void PlayGame()
    {
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
        // SceneManager.LoadScene("GameScene");
    }

    public void OpenOptions()
    {;
        // SceneManager.LoadScene("OptionsScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        
        // For testing in editor:
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}