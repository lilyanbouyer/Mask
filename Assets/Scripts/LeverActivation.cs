using UnityEngine;
public class LeverActivation : MonoBehaviour
{
    [Header("Lever Settings")]
    public Vector3 ActivatedRotation = new Vector3(90.0f, 0f, 0f);
    public Vector3 BaseRotation = Vector3.zero;
    public float smoothSpeed = 2f;
    public GameObject QTELinked;
    
    [Header("Invert Axes")]
    [Tooltip("Invert the opening direction on X axis relative to CloseRotation")]
    public bool invertX = false;
    [Tooltip("Invert the opening direction on Y axis relative to CloseRotation")]
    public bool invertY = false;
    [Tooltip("Invert the opening direction on Z axis relative to CloseRotation")]
    public bool invertZ = false;
    
    private bool isOpening = false;
    private Quaternion targetRotation;
    private QTEActivated qteScript;
    
    void Start()
    {  
        if (QTELinked != null)
        {
            qteScript = QTELinked.transform.GetComponent<QTEActivated>();
        }
    }
    
    public bool TryOpen()
    {
        if (isOpening)
        {
            Debug.Log("Lever already activated!");
            return false;
        }
        
        if (QTELinked != null && qteScript != null)
        {
            Debug.Log("Starting QTE");
            qteScript.StartQTE();
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void Update()
    {
        // Check if QTE is completed
        if (qteScript != null && qteScript.done && !isOpening)
        {
            ActivateLever();
        }
        
        Vector3 targetEuler = isOpening ? GetEffectiveOpenRotation() : BaseRotation;
        targetRotation = Quaternion.Euler(targetEuler);
        
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetRotation,
            smoothSpeed * Time.deltaTime
        );
    }
    
    // New method to activate the lever
    public void ActivateLever()
    {
        isOpening = true;
        Debug.Log("Lever activated!");
    }
    
    public bool IsOpen()
    {
        return isOpening;
    }
    
    private Vector3 GetEffectiveOpenRotation()
    {
        Vector3 effective = ActivatedRotation;
        if (invertX)
            effective.x = BaseRotation.x - (ActivatedRotation.x - BaseRotation.x);
        if (invertY)
            effective.y = BaseRotation.y - (ActivatedRotation.y - BaseRotation.y);
        if (invertZ)
            effective.z = BaseRotation.z - (ActivatedRotation.z - BaseRotation.z);
        return effective;
    }
}