using UnityEngine;

public class LeverActivation : MonoBehaviour
{
    [Header("Lever Settings")]
    // Use Vector3 to allow full rotation on all axes (x, y, z)
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
    
    void Start(){  
    }

    public bool TryOpen(){
        if (QTELinked != null){
            Debug.Log("Before QTE");
            (QTELinked.transform.GetComponent<QTEActivated>()).StartQTE();
            Debug.Log("After QTE");
            return true;
        }
        else{
            return false;
        }
    }

    public void Update()
    {
        Vector3 targetEuler = isOpening ? GetEffectiveOpenRotation() : BaseRotation;
        targetRotation = Quaternion.Euler(targetEuler);
        
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetRotation,
            smoothSpeed * Time.deltaTime
        );
    }

    public bool IsOpen()
    {
        return isOpening;
    }

    // Compute OpenRotation with optional per-axis inversion around CloseRotation
    private Vector3 GetEffectiveOpenRotation()
    {
        Vector3 effective = ActivatedRotation;
        // If invert on axis, mirror OpenRotation around CloseRotation on that axis
        if (invertX)
            effective.x = BaseRotation.x - (ActivatedRotation.x - BaseRotation.x);
        if (invertY)
            effective.y = BaseRotation.y - (ActivatedRotation.y - BaseRotation.y);
        if (invertZ)
            effective.z = BaseRotation.z - (ActivatedRotation.z - BaseRotation.z);
        return effective;
    }


}
