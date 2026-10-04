using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float crouchSpeed = 2.5f;

    [Header("Jump")]
    public float jumpHeight = 1.2f;
    public float gravity = -20f;

    [Header("Crouch")]
    public float crouchHeight = 1f;
    public float crouchTransitionSpeed = 10f;

    private CharacterController controller;
    private PlayerInputManager inputManager;

    private Vector2 moveInput;

    private float verticalVelocity;

    private float originalHeight;
    private Vector3 originalCenter;

    private bool isSprinting;
    private bool isCrouching;

        public bool IsCrouching => isCrouching;
    public bool IsSprintingRequested() => isSprinting;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputManager = GetComponent<PlayerInputManager>();

        originalHeight = controller.height;
        originalCenter = controller.center;
    }

    private void Update()
    {
        if (!inputManager.GameplayInputEnabled)
            return;
        ReadInput();
        HandleCrouch();
        HandleMovement();
    }

    private void ReadInput()
    {
        moveInput = inputManager.Player.Move.ReadValue<Vector2>();

                isSprinting = inputManager.Player.Sprint.IsPressed();
                if (PlayerStamina.Instance != null && PlayerStamina.Instance.current <= 0f)
            isSprinting = false;
        isCrouching = inputManager.Player.Crouch.IsPressed();
    }

    private void HandleMovement()
    {
        Vector3 move =
            transform.right * moveInput.x +
            transform.forward * moveInput.y;

        float speed;

        if (isCrouching)
        {
            speed = crouchSpeed;
        }
        else if (isSprinting)
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = walkSpeed;
        }

        if (controller.isGrounded)
        {
            if (verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }

            if (inputManager.Player.Jump.WasPressedThisFrame() && !isCrouching)
            {
                verticalVelocity =
                    Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = move * speed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleCrouch()
    {
        float targetHeight = isCrouching
            ? crouchHeight
            : originalHeight;

        controller.height = Mathf.Lerp(
            controller.height,
            targetHeight,
            crouchTransitionSpeed * Time.deltaTime
        );

        float heightDifference = originalHeight - controller.height;

        controller.center = new Vector3(
            originalCenter.x,
            originalCenter.y - heightDifference / 2f,
            originalCenter.z
        );
    }
}
