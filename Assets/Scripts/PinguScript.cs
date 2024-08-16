using UnityEngine;
using UnityEngine.UIElements;

public class PinguScript : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;
    public float minSpeed;
    public int gravityScale;
    public Vector3 offset;

    private float initialGravity;

    // Start is called before the first frame update
    void Start()
    {
        initialGravity = rigidBody2D.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Gravity
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody2D.gravityScale = initialGravity * gravityScale;
        } else
        {
            rigidBody2D.gravityScale = initialGravity;
        }

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

       /* Debug.Log(rigidBody2D.velocity.x);*/
    }
}
