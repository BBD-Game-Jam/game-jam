using UnityEngine;
using UnityEngine.InputSystem;

public class PinguScript : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;
    public float minSpeed;
    public float torqueAmount = 8f;
    public float rotationDamping = 0.98f;
    public float maxRotationSpeed = 400f;
    public float gravityScale;
    private float initialGravity;
    public PlayerInputActions playerControls;
    private InputAction accelerate;
    private InputAction rotate;

    private float rotationInput = 0f;

    private void Awake()
    {
        // Initialize script
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        accelerate = playerControls.Player.Accelerate;
        accelerate.Enable();
        accelerate.performed += Accelerate;
        accelerate.canceled += StopAccelerating;

        rotate = playerControls.Player.Rotate;
        rotate.Enable();
        rotate.performed += Rotate;
        rotate.canceled += OnRotateCanceled;
    }

    private void OnDisable()
    {
        accelerate.Disable();
        rotate.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        initialGravity = rigidBody2D.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        float xVelocity = Mathf.Max(rigidBody2D.velocity.x, minSpeed);
        rigidBody2D.velocity = new Vector2(xVelocity, rigidBody2D.velocity.y);
    }

    private void FixedUpdate()
    {
        // Apply torque based on input
        if (rotationInput != 0f)
        {
            rigidBody2D.AddTorque(-rotationInput * torqueAmount, ForceMode2D.Force);
        }

        // Cap the max angular velocity
        rigidBody2D.angularVelocity = Mathf.Clamp(rigidBody2D.angularVelocity, -maxRotationSpeed, maxRotationSpeed);

        // Apply damping to gradually slow down rotation over time
        rigidBody2D.angularVelocity *= rotationDamping;
    }

    private void Accelerate(InputAction.CallbackContext context)
    {
        //Debug.Log("Accelerate");
        rigidBody2D.gravityScale = initialGravity * gravityScale;
    }

    private void StopAccelerating(InputAction.CallbackContext context)
    {
        //Debug.Log("Stop Accelerating");
        rigidBody2D.gravityScale = initialGravity;
    }

    private void Rotate(InputAction.CallbackContext context)
    {
        Debug.Log("Rotate");
        rotationInput = context.ReadValue<float>();
    }

    private void OnRotateCanceled(InputAction.CallbackContext context)
    {
        rotationInput = 0f;
    }

    public void SetVelocity(float v)
    {
        minSpeed = v;
    }
}
