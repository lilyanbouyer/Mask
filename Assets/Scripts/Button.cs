using UnityEngine;
using UnityEngine.InputSystem;

public class Button : MonoBehaviour
{
    public int buttonId;
    public Simon simon;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            simon.CheckRayCast(buttonId, Mouse.current.position.ReadValue());
        }
    }
}
