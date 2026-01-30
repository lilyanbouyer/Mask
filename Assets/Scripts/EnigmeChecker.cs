using UnityEngine;
using UnityEngine.SceneManagement;

public class EnigmeChecker : MonoBehaviour
{
    public int current;
    public int needed;
    public string SceneName;
    public GameObject Door;

    public void IncrementCurrent() {
        current++;
        if(current == needed && SceneName != "")
        {
            SceneManager.LoadScene(SceneName);
        } else if(current == needed && Door != null)
        {
            // set the tag of the door to "door" insted of "locked"
            Door.tag = "Door";
        }
    }

    public void DecrementCurrent() {
        current = Mathf.Max(0, current - 1);
    }
}
