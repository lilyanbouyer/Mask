using UnityEngine;
using UnityEngine.InputSystem;

public class Interactions : MonoBehaviour
{
    public Camera playerCamera;
    
    // Nouveau Input System
    public InputActionAsset actionsAsset;
    private InputAction interactAction;
    private InputAction maskAction;
    private bool interactPressed = false;
    private bool maskPressed = false;
    public NewMonoBehaviourScript movementsScript;
    public Transform playerHand;
    private GameObject currentItem = null;
    public sanite saniteScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialisation des actions
        if (actionsAsset == null)
        {
            Debug.LogError("Assigne InputActionAsset dans l'inspecteur !");
            return;
        }
        interactAction = actionsAsset.FindAction("Interact");
        maskAction = actionsAsset.FindAction("Mask");
        if (interactAction != null)
        {
            interactAction.Enable();
        }
        if (maskAction != null)
        {
            maskAction.Enable();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (interactAction != null && interactAction.IsPressed()) {
            if (!interactPressed) {
                interactPressed = true;
                RaycastHit hit;
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 4f)) {
                    ControllerColliderHit(hit);
                }
            }
        }
        else {
            interactPressed = false;
        }

        if (maskAction != null && maskAction.IsPressed()) {
            if (!maskPressed) {
                maskPressed = true;
                saniteScript.IteractMask();
            }
        }
        else {
            maskPressed = false;
        }
    }

    private void ControllerColliderHit(RaycastHit hit)
    {
        if (hit.transform.gameObject.CompareTag("Pickup"))    
            PickupItem(hit);
    }

    public void PickupItem(RaycastHit hit)
    {
        GameObject targetItem = hit.transform.gameObject;
        currentItem = targetItem;
        currentItem.transform.SetParent(playerHand);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.localRotation = Quaternion.Euler(0, 180, 0);
        currentItem.GetComponent<Rigidbody>().isKinematic = true;
        currentItem.GetComponent<Rigidbody>().useGravity = true;
        if (currentItem.GetComponent<Collider>() != null)
            currentItem.GetComponent<Collider>().enabled = false;
    }
}
