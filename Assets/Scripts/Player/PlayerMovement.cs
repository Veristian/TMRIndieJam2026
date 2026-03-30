using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxMoveSpeed = 5f;
    [SerializeField] private float maxSprintSpeed = 10f;
    [SerializeField] private float moveForce = 5f;
    [SerializeField] private float waterFriction = 5f;
    [SerializeField] private float sprintResource = 3f; // Total sprint resource
    [SerializeField] private float sprintDepletionRate = 1f; // Resource depletion
    [SerializeField] private float sprintRecoveryRate = 1.5f; // Resource recovery when not sprinting
    private Rigidbody rb;
    private float currentSprintResource;
    private bool canSprint = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentSprintResource = sprintResource;
    }

    private void FixedUpdate()
    {
        HandlePlayerMovement(InputManager.movementInput);
    }

    private void HandlePlayerMovement(Vector2 movementInput)
    {
        
        if (InputManager.sprintIsHeld && currentSprintResource > 0 && canSprint)
        {
            currentSprintResource -= sprintDepletionRate * Time.fixedDeltaTime;
            if (currentSprintResource < 0)
            {
                canSprint = false;
                currentSprintResource = 0;
            }
        }
        else
        {
            currentSprintResource += sprintRecoveryRate * Time.fixedDeltaTime;
            if (currentSprintResource > sprintResource)
            {
                canSprint = true;
                currentSprintResource = sprintResource;
            } 
        }
        if (Mathf.Abs(movementInput.x) < 0.1f && Mathf.Abs(movementInput.y) < 0.1f)
        {
            // Apply water friction when no input is given
            Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            Vector3 frictionForce = -horizontalVelocity * waterFriction;
            rb.AddForce(frictionForce, ForceMode.Acceleration);
            return;
        }

        float currentMaxSpeed = (InputManager.sprintIsHeld && canSprint) ? maxSprintSpeed : maxMoveSpeed;
        
        Vector3 desiredVelocity = new Vector3(movementInput.x, 0, movementInput.y) * currentMaxSpeed;
        Vector3 velocityChange = desiredVelocity - rb.linearVelocity;
        velocityChange.y = 0; // Don't change vertical velocity
        rb.AddForce(velocityChange * moveForce, ForceMode.Acceleration);

    }

}
