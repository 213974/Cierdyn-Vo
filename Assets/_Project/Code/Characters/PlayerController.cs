using UnityEngine;
using UnityEngine.InputSystem; // Required for the new Input System

public class PlayerController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Cinemachine.CinemachineVirtualCamera virtualCamera;

    private CharacterMonoBehaviour activeCharacter;
    private CharacterMonoBehaviour followerCharacter;

    // This is where the GameManager will give us our spawned characters.
    public void PossessSquad(CharacterMonoBehaviour cierdyn, CharacterMonoBehaviour elara, CharacterId activeId)
    {
        if (activeId == CharacterId.Cierdyn)
        {
            SetActiveCharacter(cierdyn, elara);
        }
        else
        {
            SetActiveCharacter(elara, cierdyn);
        }
    }

    // Input System Actions - these will be called by the Player Input component.
    public void OnMove(InputAction.CallbackContext context)
    {
        if (activeCharacter != null)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            activeCharacter.SetMoveInput(new Vector3(moveInput.x, 0, moveInput.y));
        }
    }

    public void OnSwitchCharacter(InputAction.CallbackContext context)
    {
        if (context.performed) // Only trigger on the button press
        {
            // The active character becomes the follower, and vice-versa.
            SetActiveCharacter(followerCharacter, activeCharacter);
        }
    }

    void Update()
    {
        // Every frame, tell the active character to move itself.
        if (activeCharacter != null)
        {
            activeCharacter.UpdateMovement();
        }

        // And tell the follower to follow the active character.
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

        // Tell the camera to follow the new active character
        if (virtualCamera != null)
        {
            virtualCamera.Follow = activeCharacter.transform;
            virtualCamera.LookAt = activeCharacter.transform;
        }
    }
}