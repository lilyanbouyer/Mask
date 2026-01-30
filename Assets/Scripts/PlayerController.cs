using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject playerHand;
	public GameObject player;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 4f;
    public float gravity = 10f;

    public ItemPickup itemPickup;
    public DoorController doorController;

    public float lookSpeed = 2.5f;
    public float lookXLimit = 70f;

    public bool HasKey { get; set; }

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;
    public LightController lightController;

    
    CharacterController characterController;
    void Start()
    {
        if (PlayerPrefs.HasKey("walkSpeed")) {
            walkSpeed = PlayerPrefs.GetFloat("walkSpeed");
        }
        if (PlayerPrefs.HasKey("sprintSpeed")) {
            runSpeed = PlayerPrefs.GetFloat("sprintSpeed");
        }
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        itemPickup = GetComponent<ItemPickup>();
        doorController = GetComponent<DoorController>();
        lightController = GetComponent<LightController>();
        HasKey = false;
    }

    void Update()
    {
		if (PlayerPrefs.GetInt("gameIsPaused") == 1)
			canMove = false;
		else
			canMove = true;
        #region Handles Movment
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
		bool isCrouching = Input.GetKey(KeyCode.LeftControl);
		if (isCrouching) {
			isRunning = false;
			player.transform.localScale = new Vector3(1, 0.8f, 1);
		} else
			player.transform.localScale = new Vector3(1, 1.6f, 1);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        #endregion

        #region Handles Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            moveDirection.y = jumpPower;
        else
            moveDirection.y = movementDirectionY;

        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;

        #endregion

        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        #endregion
        
        if (Input.GetKeyDown(KeyCode.E)) {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 4f)) {
                ControllerColliderHit(hit);
            }
            else if (itemPickup.GetCurrentItem())
                itemPickup.DropItem();
        } else if (Input.GetKeyDown(KeyCode.F12)) {
            SceneManager.LoadScene("Test_Scene");
        }
        if (doorController && doorController.GetCurrentDoor())
            doorController.HandleDoor();
    }

    private void ControllerColliderHit(RaycastHit hit)
    {
        
        if (hit.transform.gameObject.CompareTag("Door"))
            doorController.OpenCloseDoor(hit);
        else if (hit.transform.gameObject.CompareTag("Pickup") && !itemPickup.GetCurrentItem())    
            itemPickup.PickupItem(hit);
        else if (hit.transform.gameObject.CompareTag("Placement") && itemPickup.GetCurrentItem()) {
            if (hit.transform.childCount == 0) {
                hit.transform.GetComponent<ItemPlacement>().SetCurrentItem(itemPickup.GetCurrentItem());
                hit.transform.GetComponent<ItemPlacement>().PlaceItem(hit);
                itemPickup.SetCurrentItem(null);
            }
        } else if (itemPickup.GetCurrentItem())
            itemPickup.DropItem();
        else if (hit.transform.gameObject.CompareTag("Switch"))
            lightController.ToggleLightWithend(hit.transform.gameObject.name, hit.transform.gameObject);
    }
}