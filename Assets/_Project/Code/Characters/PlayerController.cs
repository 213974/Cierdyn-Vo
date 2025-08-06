using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CameraController mainCameraController;

    private CharacterMonoBehaviour activeCharacter;
    private CharacterMonoBehaviour followerCharacter;
    private PlayerInputActions playerActions;

    void Awake()
    {
        playerActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerActions.Player.Move.performed += OnMove;
        playerActions.Player.Move.canceled += OnMove;
        playerActions.Player.SwitchCharacter.performed += OnSwitchCharacter;
        playerActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerActions.Player.Move.performed -= OnMove;
        playerActions.Player.Move.canceled -= OnMove;
        playerActions.Player.SwitchCharacter.performed -= OnSwitchCharacter;
        playerActions.Player.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Lock the mouse by default when the game starts.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PossessSquad(CharacterMonoBehaviour cierdyn, CharacterMonoBehaviour elara, CharacterId activeId)
    {
        if (activeId == CharacterId.Cierdyn) SetActiveCharacter(cierdyn, elara);
        else SetActiveCharacter(elara, cierdyn);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();

        // Don't process movement if the cursor is unlocked (for future UI states)
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            activeCharacter?.SetMoveInput(Vector3.zero);
            return;
        }

        if (activeCharacter != null)
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * moveInput.y + cameraRight * moveInput.x;
            activeCharacter.SetMoveInput(moveDirection);
        }
    }

    private void OnSwitchCharacter(InputAction.CallbackContext context)
    {
        SetActiveCharacter(followerCharacter, activeCharacter);
    }

    void Update()
    {
        if (followerCharacter != null && activeCharacter != null)
        {
            followerCharacter.Follow(activeCharacter.transform);
        }
    }

    private void SetActiveCharacter(CharacterMonoBehaviour newActive, CharacterMonoBehaviour newFollower)
    {
        activeCharacter = newActive;
        followerCharacter = newFollower;

        activeCharacter.SetAsActive();
        followerCharacter.SetAsFollower();

        if (mainCameraController != null)
        {
            mainCameraController.SetTarget(activeCharacter.transform);
        }
    }
}