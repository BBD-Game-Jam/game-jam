using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PinguScript : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;
    public Camera cam;
    public float minSpeed;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
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

        // Update the camera's position to follow the object
        cam.transform.position = transform.position + offset;
    }
}
