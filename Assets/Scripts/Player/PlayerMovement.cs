using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;


    private void Update()
    {
        HandlePlayerMovement(InputManager.movementInput);
    }

    private void HandlePlayerMovement(Vector2 movementInput)
    {
        if (Mathf.Abs(movementInput.x) < 0.1f && Mathf.Abs(movementInput.y) < 0.1f)
            return;
        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y) * moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }

}
