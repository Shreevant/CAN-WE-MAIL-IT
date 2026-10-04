using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("References")]
    public Transform cameraHolder;

    [Header("Look")]
    public float sensitivity = 150f;
    public float minPitch = -85f;
    public float maxPitch = 85f;

    [Header("Crouch Camera")]
    public float crouchCameraOffset = 0.5f;
    public float cameraCrouchSpeed = 10f;

    private PlayerInputManager inputManager;
    private PlayerMovement playerMovement;

    private float pitch;

    private Vector3 standingCameraPosition;

    private void Awake()
    {
        inputManager = GetComponent<PlayerInputManager>();
        playerMovement = GetComponent<PlayerMovement>();

        standingCameraPosition = cameraHolder.localPosition;
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!inputManager.GameplayInputEnabled)
            return;
        HandleLook();
        HandleCrouchCamera();
    }

    private void HandleLook()
    {
        Vector2 lookInput =
            inputManager.Player.Look.ReadValue<Vector2>();

        float mouseX =
            lookInput.x * sensitivity * Time.deltaTime;

        float mouseY =
            lookInput.y * sensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        pitch -= mouseY;

        pitch = Mathf.Clamp(
            pitch,
            minPitch,
            maxPitch
        );

        cameraHolder.localRotation =
            Quaternion.Euler(pitch, 0f, 0f);
    }

    private void HandleCrouchCamera()
    {
        Vector3 targetPosition =
            standingCameraPosition;

        if (playerMovement.IsCrouching)
        {
            targetPosition.y -= crouchCameraOffset;
        }

        cameraHolder.localPosition =
            Vector3.Lerp(
                cameraHolder.localPosition,
                targetPosition,
                cameraCrouchSpeed * Time.deltaTime
            );
    }
}
