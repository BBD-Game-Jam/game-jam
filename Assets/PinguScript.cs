using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PinguScript : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;
    public Camera cam;
    public float speed;
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
        // Get the object's forward and right directions based on its rotation
        Vector2 forward = transform.right;    // Forward direction (local up)
        Vector2 right = -transform.up;   // Right direction (local right)

        // Apply movement relative to the object's rotation
        if (Input.GetKey(KeyCode.W))
        {
            rigidBody2D.velocity = forward * speed;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rigidBody2D.velocity = -right * speed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rigidBody2D.velocity = -forward * speed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rigidBody2D.velocity = right * speed;
        }

        // Clamp rotation between -90 and 90 degrees
        float clampedZRotation = ClampRotationZ(transform.eulerAngles.z);
        transform.rotation = Quaternion.Euler(0, 0, clampedZRotation);

        // Update the camera's position to follow the object
        cam.transform.position = transform.position + offset;
    }

    // Clamps the rotation to a range between -90 and 90 degrees
    private float ClampRotationZ(float zRotation)
    {
        if (zRotation > 270)
        {
            zRotation -= 360;
        }
        return Mathf.Clamp(zRotation, 0f, 180f);
    }
}
