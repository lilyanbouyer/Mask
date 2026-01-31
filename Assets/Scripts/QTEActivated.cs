using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QTEActivated : MonoBehaviour
{
    public bool done { get; private set; } = false;
    public GameObject qteUI;   // assign QTE_Background
    public Key interactKey = Key.E;
    private bool active = false;
    public Slider slider;
    public Image greenZone;
    //private System.Random rnd = new System.Random();
    
    void Start()
    {
        qteUI.SetActive(false);
    }

    void Update()
    {
        if (!active) return;

        if (Keyboard.current[interactKey].wasPressedThisFrame)
        {
            Debug.Log("QTE success!");
            GenerateQTE();
            CompleteQTE();
        }
    }

    private void GenerateQTE() {
        float value = Random.Range(0.0f, 1.0f);
        Debug.Log(value);
        
        //slider. value = value;

        RectTransform rt = slider.GetComponent<RectTransform>();
        float greenPos = rt.rect.width * value - (rt.rect.width/2);

        Debug.Log("greenZone Pos: " + greenPos);

        RectTransform greenRT = greenZone.GetComponent<RectTransform>();
        Vector2 anchored = greenRT.anchoredPosition;
        anchored.x = greenPos;
        greenRT.anchoredPosition = anchored;

    }

    public void CompleteQTE()
    {
        done = true;
        //qteUI.SetActive(false);
        Debug.Log("QTE completed");
    }

    public void ResetQTE()
    {
        done = false;
    }

    public void StartQTE()
    {
        active = true;
        //qteUI.SetActive(true);
    }
}
