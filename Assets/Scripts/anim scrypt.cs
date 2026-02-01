using System.Threading.Tasks;
using UnityEngine;
using System.Collections;

public class animscrypt : MonoBehaviour
{
    public float waitTime = 5f;
    public string sceneToLoad;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async Task Start()
    {
        StartCoroutine(LoadSceneAfterDelay());
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(waitTime);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
