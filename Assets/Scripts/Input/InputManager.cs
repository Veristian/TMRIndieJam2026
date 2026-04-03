using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
public class InputManager : Singleton<InputManager>
{
    public static PlayerInput playerInput;
    public static Vector2 touchPosition;

    //System Callbacks
    public static event System.Action<Vector2> PlayerMovement;
    public static event System.Action OnInteract;
    public static event System.Action OnSprint;

    //Public Value
    public static Vector2 movementInput;
    public static bool interactWasPressedThisFrame;
    public static bool interactWasReleasedThisFrame;
    public static bool interactIsHeld;

    public static bool pauseWasPressedThisFrame;

    public static bool sprintIsHeld;


    protected override void Awake()
    {

        base.Awake();
        playerInput = GetComponent<PlayerInput>();
        
        playerInput.actions["Move"].performed += ctx =>
        {
            touchPosition = ctx.ReadValue<Vector2>();
            PlayerMovement?.Invoke(touchPosition);
        };
        playerInput.actions["Interact"].performed += ctx =>
        {
            OnInteract?.Invoke();
        };
        playerInput.actions["Sprint"].performed += ctx =>
        {
            OnSprint?.Invoke();
        };

    }

    private void Update()
    {
        movementInput = playerInput.actions["Move"].ReadValue<Vector2>();
        interactWasPressedThisFrame = playerInput.actions["Interact"].WasPressedThisFrame();
        interactWasReleasedThisFrame = playerInput.actions["Interact"].WasReleasedThisFrame();
        interactIsHeld = playerInput.actions["Interact"].IsPressed();
        sprintIsHeld = playerInput.actions["Sprint"].IsPressed();
        pauseWasPressedThisFrame = playerInput.actions["Pause"].WasPressedThisFrame();

    }


}
