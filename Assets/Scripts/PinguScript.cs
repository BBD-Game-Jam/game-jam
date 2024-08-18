using UnityEngine;
using UnityEngine.InputSystem;

public class PinguScript : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;
    public float minSpeed;
    public int gravityScale;
    private float initialGravity;
    public PlayerInputActions playerControls;
    private InputAction accelerate;
    private InputAction rotate;
    public float rotationSpeed = 1;

    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;

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
        float xVelocity;
        float yVelocity;
        yVelocity = rigidBody2D.velocity.y;
        if (rigidBody2D.velocity.x < minSpeed)
        {
            xVelocity = minSpeed;
        }
        else
        {
            xVelocity = rigidBody2D.velocity.x;
        }
        rigidBody2D.velocity = new Vector2(xVelocity, yVelocity);
    }

    // Update is called once per frame
    void Update()
    {
        float xVelocity;
        float yVelocity;
        yVelocity = rigidBody2D.velocity.y;
        if (rigidBody2D.velocity.x < minSpeed)
        {
            xVelocity = minSpeed;
        }
        else
        {
            xVelocity = rigidBody2D.velocity.x;
        }
        rigidBody2D.velocity = new Vector2(xVelocity, yVelocity);
    }

    private void FixedUpdate()
    {
        // Rotate the object continuously based on the input
        if (isRotatingLeft)
        {
            rigidBody2D.rotation += 5f*rotationSpeed; // Adjust the rotation speed as needed
        }
        else if (isRotatingRight)
        {
            rigidBody2D.rotation -= 5f*rotationSpeed; // Adjust the rotation speed as needed
        }
    }

    private void Accelerate(InputAction.CallbackContext context)
    {
        Debug.Log("Accelerate");
        rigidBody2D.gravityScale = initialGravity * gravityScale;
    }

    private void StopAccelerating(InputAction.CallbackContext context)
    {
        Debug.Log("Stop Accelerating");
        rigidBody2D.gravityScale = initialGravity;
    }

    private void Rotate(InputAction.CallbackContext context)
    {
        Debug.Log("We rotated");
        // Debug.Log(context);
        float rotationValue = context.ReadValue<float>();

        if (rotationValue == -1)
        {
            isRotatingLeft = true;
        }
        else if (rotationValue == 1)
        {
            isRotatingRight = true;
        }

        rigidBody2D.angularVelocity = 0;

    }
    private void OnRotateCanceled(InputAction.CallbackContext context)
    {
        isRotatingLeft = false;
        isRotatingRight = false;
    }
}
