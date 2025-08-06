using UnityEngine;
using UnityEngine.InputSystem; // We now need the new Input System here.

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float mouseSensitivity = 3f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 3f, -6f);
    [SerializeField] private float pitchMin = -20f;
    [SerializeField] private float pitchMax = 80f;

    private Transform target;
    private float yaw = 0f;
    private float pitch = 15f;

    // We need a reference to the same Input Actions asset.
    private PlayerInputActions playerActions;

    void Awake()
    {
        playerActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        // Subscribe to the "Look" action.
        playerActions.Player.Look.performed += OnLook;
        playerActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerActions.Player.Look.performed -= OnLook;
        playerActions.Player.Disable();
    }

    // This method is called by the Input System when the mouse moves.
    private void OnLook(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>();
        yaw += mouseDelta.x * mouseSensitivity * Time.deltaTime;
        pitch -= mouseDelta.y * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void LateUpdate()
    {
        if (target == null || Cursor.lockState != CursorLockMode.Locked)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 targetPosition = target.position + targetRotation * offset;

        transform.position = targetPosition;
        transform.LookAt(target.position);
    }
}