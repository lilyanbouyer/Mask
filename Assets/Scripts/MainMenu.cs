using UnityEngine;
using UnityEngine.SceneManagement;

    //SceneManager.LoadSceneAscync(sceneToLoad, LoadSceneMode.Single);
    //Application.Quit();

public class MainMenu : MonoBehaviour
{
    public string sceneToLoad;

    public void PlayGame()
    {
        Debug.Log("Press Start");
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
        // SceneManager.LoadScene("GameScene");
    }

    public void OpenOptions()
    {
        Debug.Log("Options clicked");
        // SceneManager.LoadScene("OptionsScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
        
        // For testing in editor:
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}