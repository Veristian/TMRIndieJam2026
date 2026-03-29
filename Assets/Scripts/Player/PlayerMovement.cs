using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxMoveSpeed = 5f;
    [SerializeField] private float moveForce = 5f;
    [SerializeField] private float waterFriction = 5f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        HandlePlayerMovement(InputManager.movementInput);
    }

    private void HandlePlayerMovement(Vector2 movementInput)
    {
        if (Mathf.Abs(movementInput.x) < 0.1f && Mathf.Abs(movementInput.y) < 0.1f)
        {
            // Apply water friction when no input is given
            Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            Vector3 frictionForce = -horizontalVelocity * waterFriction;
            rb.AddForce(frictionForce, ForceMode.Acceleration);
            return;
        }
        
        Vector3 desiredVelocity = new Vector3(movementInput.x, 0, movementInput.y) * maxMoveSpeed;
        Vector3 velocityChange = desiredVelocity - rb.linearVelocity;
        velocityChange.y = 0; // Don't change vertical velocity
        rb.AddForce(velocityChange * moveForce, ForceMode.Acceleration);

    }

}
