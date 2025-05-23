using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 3.0f;


    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float gravityMultiplier = 1.0f;


    [Header("Look Parameters")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float upDownLookRange = 80f;


    [Header ("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera mainCamera;

    [SerializeField] private PlayerInputHandler playerInputHandler;
    public GameObject thisObject;

    [Header("External References")]
    [SerializeField] private PauseController pauseCtrl;

    private Vector3 currentMovement;
    private float verticalRotation;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    // Update is called once per frame
    void Update()
    {

        HandleMovement();
        HandleRotation();
    }


    private Vector3 CalculateWorldDirection()
    {
        Vector3 inputDirection = new Vector3(playerInputHandler.MovementInput.x, 0f, playerInputHandler.MovementInput.y);
        Vector3 worldDirection = transform.TransformDirection(inputDirection);
        return worldDirection.normalized;
    }


    private void HandleJumping()
    {
        if (characterController.isGrounded && thisObject.tag == "Player")
        {
            currentMovement.y = -0.5f;


            if (playerInputHandler.JumpTriggered)
            {
                currentMovement.y = jumpForce;
            }
        }
        else
        {
            currentMovement.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }
    }


    private void HandleMovement()
    {
        if (thisObject.tag == "Player")
        {
            Vector3 worldDirection = CalculateWorldDirection();
            currentMovement.x = worldDirection.x * walkSpeed;
            currentMovement.z = worldDirection.z * walkSpeed;


            HandleJumping();
            characterController.Move(currentMovement * Time.deltaTime);
        }
    }


    private void ApplyHorizontalRotation(float rotationAmount)
    {
        transform.Rotate(0, rotationAmount, 0);
    }


    private void ApplyVerticalRotation(float rotationAmount)
    {
        verticalRotation = Mathf.Clamp(verticalRotation - rotationAmount, -upDownLookRange, upDownLookRange);
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }


    private void HandleRotation()
    {
        if(!pauseCtrl.IsPaused)
        {
            float mouseXRotation = playerInputHandler.RotationInput.x * mouseSensitivity;
            float mouseYRotation = playerInputHandler.RotationInput.y * mouseSensitivity;


            ApplyHorizontalRotation(mouseXRotation);
            ApplyVerticalRotation(mouseYRotation);
        }

    }
}
