using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QTEActivated : MonoBehaviour{
    public bool done { get; private set; } = false;
    public GameObject qteUI;
    public Key interactKey = Key.E;
    private bool active = true;
    public Slider slider;
    public Image greenZone;
    private float CurrentValue;
    private float sliderSpeed = 1;
    private bool movingRight = true;
    private float time;
    //private System.Random rnd = new System.Random();
    
    void Start(){
        qteUI.SetActive(false);
    }

    void Update(){
        if (!active) return;
        SliderBackAndForth();
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

    private void SliderBackAndForth(){
        if (!active) return;

        time += Time.deltaTime * sliderSpeed;

        float raw = Mathf.PingPong(time, 1f);
        slider.value = Mathf.SmoothStep(0f, 1f, raw);
    }

    private bool IsInGreenZone(){
        RectTransform handleRT = slider.handleRect;
        RectTransform greenRT = greenZone.GetComponent<RectTransform>();

        float handleX = handleRT.TransformPoint(handleRT.rect.center).x;

        float greenMin = greenRT.TransformPoint(
            new Vector3(-greenRT.rect.width / 2f, 0f, 0f)
        ).x;

        float greenMax = greenRT.TransformPoint(
            new Vector3(greenRT.rect.width / 2f, 0f, 0f)
        ).x;

        return handleX >= greenMin && handleX <= greenMax;
    }


    private void GenerateQTE() {
        CurrentValue = Random.Range(0.0f, 1.0f);
        Debug.Log(CurrentValue);
        RectTransform rt = slider.GetComponent<RectTransform>();
        float greenPos = (rt.rect.width-20) * CurrentValue - ((rt.rect.width-20)/2);
        RectTransform greenRT = greenZone.GetComponent<RectTransform>();
        Vector2 anchored = greenRT.anchoredPosition;
        anchored.x = greenPos;
        greenRT.anchoredPosition = anchored;
    }

    public void CompleteQTE(){
        done = true;
        qteUI.SetActive(false);
        active = false;
    }

    public void ResetQTE(){
        done = false;
        GenerateQTE();
    }

    public void StartQTE(){
        active = true;
        qteUI.SetActive(true);
        GenerateQTE();
    }
}
