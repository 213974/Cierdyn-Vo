using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMonoBehaviour : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float followDistance = 3f;

    private CharacterController controller;
    private Vector3 moveInput;
    private bool isActive;

    public CharacterId CharacterId { get; private set; }

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // All movement logic is now self-contained in the character's update.
        if (isActive)
        {
            HandleActiveMovement();
        }
    }

    public void Initialize(CharacterStateData data)
    {
        this.CharacterId = data.id;
        transform.position = data.position;
        gameObject.name = data.id.ToString();
    }

    public void SetAsActive() => isActive = true;
    public void SetAsFollower() => isActive = false;

    public void SetMoveInput(Vector3 input)
    {
        moveInput = input;
    }

    private void HandleActiveMovement()
    {
        // Apply movement
        controller.Move(moveInput * moveSpeed * Time.deltaTime);

        // Apply rotation to face movement direction
        if (moveInput.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void Follow(Transform target)
    {
        if (isActive) return;

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > followDistance)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            controller.Move(direction * (moveSpeed * 0.8f) * Time.deltaTime); // Follow at 80% speed

            // Also rotate to look at the leader
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}