using UnityEngine;
using UnityEngine.InputSystem;

public class Button : MonoBehaviour
{
    public int buttonId;
    public Simon simon;
    public Camera playerCamera;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            //Debug.Log("Left click detected");

            Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    //Debug.Log("Button pressed: " + buttonId);
                    simon.CheckInput(buttonId);
                }
            }
        }
    }
}
