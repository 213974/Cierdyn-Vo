using UnityEngine;

// We now require a CharacterController for robust, physics-based movement.
[RequireComponent(typeof(CharacterController))]
public class CharacterMonoBehaviour : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 moveInput;
    private Transform followTarget;
    private bool isActive;

    public CharacterId CharacterId { get; private set; }

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void Initialize(CharacterStateData data)
    {
        this.CharacterId = data.id;
        this.transform.position = data.position;
        this.gameObject.name = data.id.ToString();
    }

    // --- State Control ---
    public void SetAsActive() => isActive = true;
    public void SetAsFollower() => isActive = false;

    // --- Movement Logic ---
    public void SetMoveInput(Vector3 input)
    {
        moveInput = input;
    }

    public void UpdateMovement()
    {
        if (!isActive) return;
        controller.Move(moveInput * 5f * Time.deltaTime); // 5f is a placeholder for moveSpeed
    }

    public void Follow(Transform target)
    {
        if (isActive) return;

        float distance = Vector3.Distance(transform.position, target.position);
        // Only start following if we are a certain distance away.
        if (distance > 3f)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            controller.Move(direction * 4f * Time.deltaTime); // Follow at a slightly slower speed
        }
    }
}