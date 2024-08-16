using System;
using UnityEngine;
using UnityEngine.U2D;

public class TerainGenerator : MonoBehaviour
{
    public SpriteShapeController shape;
    public Rigidbody2D terrainRG;
    public Transform character;  // Reference to the character's transform
    public GameObject pingu;
    public Camera cam;
    public int distanceBetweenPoints = 5;
    public int heightOffset = 20;
    public int curvature = 30;
    public float generationThreshold = 50f;  // Distance from the edge of the terrain to trigger new generation
    public float deletionThreshold = 50f;  // Increased distance behind the character to delete points

    private float lowestPointYPos;
    private int lastPointIndex = 4;
    private readonly int firstPointIndex = 1;

    void Start()
    {
        shape = GetComponent<SpriteShapeController>();
        lowestPointYPos = shape.spline.GetPosition(0).y;
        if (cam == null)
        {
            cam = Camera.main;  // Fallback to main camera if not assigned
        }
    }

    void Update()
    {
        if (cam != null && shape != null)
        {
            // Calculate camera's left and right edges in world space
            float cameraLeftEdgeX = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane)).x;
            float cameraRightEdgeX = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane)).x;

            // Check if new points need to be generated
            float lastPointXPos = shape.spline.GetPosition(lastPointIndex).x;
            if (cameraRightEdgeX / 4 > lastPointXPos - generationThreshold) // Don't ask me why I divide by four its the only way it works XD
            {
                GenerateNewPoint();
            }

            // Check if old points need to be deleted
            float secondPointXpos = shape.spline.GetPosition(firstPointIndex+1).x;
            if (cameraLeftEdgeX / 4 > secondPointXpos + deletionThreshold)
            {
                DeleteOldPoint();
            }
        }

        //Center();
    }

    void Center()
    {
        Rigidbody2D rg = pingu.GetComponent<Rigidbody2D>();
        float xVelocity = rg.velocity.x;
        /* Debug.Log($"Hi {xVelocity}");*/
        /*transform.Translate(Vector3.left * xVelocity * Time.deltaTime);*/
        terrainRG.velocity = new Vector2(-xVelocity, 0);
    }

    void GenerateNewPoint()
    {
        // Generate new point positions using Perlin noise
        float xPos = shape.spline.GetPosition(lastPointIndex).x + distanceBetweenPoints;
        float yPos = heightOffset * Mathf.PerlinNoise(lastPointIndex * UnityEngine.Random.Range(5.0f, 15.0f), 0);

        // Insert point
        shape.spline.InsertPointAt(lastPointIndex + 1, new Vector3(xPos, yPos, 0));
        shape.spline.SetTangentMode(lastPointIndex + 1, ShapeTangentMode.Continuous);
        shape.spline.SetLeftTangent(lastPointIndex + 1, new Vector3(-curvature / 10, 0, 0));
        shape.spline.SetRightTangent(lastPointIndex + 1, new Vector3(curvature / 10, 0, 0));
        lastPointIndex++;

        // Update bottom right point to keep shape
        int bottomRightPointIndex = shape.spline.GetPointCount() - 1;
        shape.spline.SetPosition(bottomRightPointIndex, new Vector3(xPos, shape.spline.GetPosition(bottomRightPointIndex).y, 0));
    }

    void DeleteOldPoint()
    {
        if (shape.spline.GetPointCount() > 4 && firstPointIndex < lastPointIndex)  // Ensure we have enough points to delete
        {
            // Remove the leftmost upper point
            shape.spline.RemovePointAt(firstPointIndex);

            // Update the bottom left point to keep it aligned with the new first upper point
            int bottomLeftPointIndex = 0;
            float newXPos = shape.spline.GetPosition(firstPointIndex).x;  // Access the new first point after deletion
            shape.spline.SetPosition(bottomLeftPointIndex, new Vector3(newXPos, lowestPointYPos, 0));

            // Adjust the indices to account for the deletion
            lastPointIndex--;
        }
    }
}
