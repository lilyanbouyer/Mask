using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QTEActivated : MonoBehaviour
{
    public bool done { get; private set; } = false;
    public GameObject qteUI;   // assign QTE_Background
    public Key interactKey = Key.E;
    private bool active = true;
    public Slider slider;
    public Image greenZone;
    private float CurrentValue;
    private float sliderSpeed = 1;
    private bool movingRight = true;
    //private System.Random rnd = new System.Random();
    
    void Start()
    {
        GenerateQTE();
        //qteUI.SetActive(false);
    }

    void Update()
    {
        //if (!active) return;
        SliderBackAndForth();
        /*
        if (Keyboard.current[interactKey].wasPressedThisFrame)
        {
            Debug.Log("QTE success!");
            GenerateQTE();
            CompleteQTE();
        }*/
        if (Keyboard.current[interactKey].wasPressedThisFrame){
            if (IsInGreenZone()){
                Debug.Log("QTE success!");
                CompleteQTE();
            }
            else{
                Debug.Log("QTE failed!");
                ResetQTE();
            }
        }

    }
    
    private void SliderBackAndForth()
    {
        if (!active) return;

        if (slider.value >= 1f)
            movingRight = false;
        else if (slider.value <= 0f)
            movingRight = true;

        float dir = movingRight ? 1f : -1f;
        slider.value += dir * sliderSpeed * Time.deltaTime;
    }

    private bool IsInGreenZone(){
        float tolerance = 0.05f; // width of green zone
        return Mathf.Abs(slider.value - CurrentValue) <= tolerance;
    }


    private void GenerateQTE() {
        CurrentValue = Random.Range(0.0f, 1.0f);
        Debug.Log(CurrentValue);

        RectTransform rt = slider.GetComponent<RectTransform>();
        float greenPos = (rt.rect.width-20) * CurrentValue - ((rt.rect.width-20)/2);

        Debug.Log("greenZone Pos: " + greenPos);

        RectTransform greenRT = greenZone.GetComponent<RectTransform>();
        Vector2 anchored = greenRT.anchoredPosition;
        anchored.x = greenPos;
        greenRT.anchoredPosition = anchored;

    }

    public void CompleteQTE()
    {
        done = true;
        qteUI.SetActive(false);
        active = false;
        Debug.Log("QTE completed");
    }

    public void ResetQTE()
    {
        done = false;
        GenerateQTE();
    }

    public void StartQTE()
    {
        active = true;
        //qteUI.SetActive(true);
    }
}
