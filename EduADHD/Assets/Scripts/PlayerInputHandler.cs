using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string movement = "Movement";
    [SerializeField] private string rotation = "Rotation";
    [SerializeField] private string jump = "Jump";

    [Header("External References")]
    [SerializeField] private ClassroomInteraction classroomInteraction;

    private InputAction movementAction;
    private InputAction rotationAction;
    private InputAction jumpAction;

    public Vector2 MovementInput { get; private set; }
    public Vector2 RotationInput { get; private set; }
    public bool JumpTriggered { get; private set; }

    public bool IsMovementEnabled => movementAction.enabled && jumpAction.enabled;

    private void Awake()
    {
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);

        movementAction = mapReference.FindAction(movement);
        rotationAction = mapReference.FindAction(rotation);
        jumpAction = mapReference.FindAction(jump);

        SubscribeActionValuesToInputEvents();
    }

    private void OnEnable()
    {
        playerControls.FindActionMap(actionMapName).Enable();
        movementAction.Enable();
        rotationAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        jumpAction.Disable();
        rotationAction.Disable();
        movementAction.Disable();
        playerControls.FindActionMap(actionMapName).Disable();
    }

    private void Update()
    {
        if (classroomInteraction != null)
        {
            if (classroomInteraction.PlayerIsSeated && IsMovementEnabled)
            {
                SetMovementEnabled(false);
            }
            else if (!classroomInteraction.PlayerIsSeated && !IsMovementEnabled)
            {
                SetMovementEnabled(true);
            }
        }
    }

    private void SubscribeActionValuesToInputEvents()
    {
        movementAction.performed += inputInfo => MovementInput = inputInfo.ReadValue<Vector2>();
        movementAction.canceled += inputInfo => MovementInput = Vector2.zero;

        rotationAction.performed += inputInfo => RotationInput = inputInfo.ReadValue<Vector2>();
        rotationAction.canceled += inputInfo => RotationInput = Vector2.zero;

        jumpAction.performed += inputInfo => JumpTriggered = true;
        jumpAction.canceled += inputInfo => JumpTriggered = false;
    }

    public void SetMovementEnabled(bool enabled)
    {
        if (enabled)
        {
            movementAction.Enable();
            jumpAction.Enable();
        }
        else
        {
            movementAction.Disable();
            jumpAction.Disable();
            MovementInput = Vector2.zero;
            JumpTriggered = false;
        }
    }
}