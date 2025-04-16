using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ShipController : MonoBehaviour
{
    public float baseSpeed = 50f;        // Initial speed when key is pressed
    public float maxSpeed = 200f;        // Max speed cap
    public float acceleration = 10f;     // Acceleration per second
    public float turnSpeed = 20f;        // Rotation speed

    private Rigidbody rb;
    private InputSystem_Actions inputActions;
    private Vector2 moveInput;
    private float currentSpeed = 0f;
    private bool accelerating = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.MoveShip.performed += OnMove;
        inputActions.Player.MoveShip.canceled += OnMove;
    }

    void OnDisable()
    {
        inputActions.Player.MoveShip.performed -= OnMove;
        inputActions.Player.MoveShip.canceled -= OnMove;
        inputActions.Disable();
    }

    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        // If there's new forward/backward input, reset speed to base
        if (Mathf.Abs(moveInput.y) > 0.1f)
        {
            if (!accelerating)
            {
                currentSpeed = baseSpeed;
                accelerating = true;
            }
        }
        else
        {
            currentSpeed = 0f;
            accelerating = false;
        }
    }

    void FixedUpdate()
    {
        // --- Steering ---
        float turn = moveInput.x * turnSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turn, 0f));

        // --- Movement ---
        if (accelerating)
        {
            currentSpeed += acceleration * Time.fixedDeltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
        }

        // Move forward/backward while preserving vertical (Y) velocity
        Vector3 forwardMovement = transform.forward * moveInput.y * currentSpeed;
        forwardMovement.y = rb.linearVelocity.y;
        rb.linearVelocity = forwardMovement;
    }
}
