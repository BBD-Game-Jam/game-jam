using UnityEngine;
using UnityEngine.U2D;
using System;

public class SandGenerator : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;
    public Transform character;  // Reference to the character's transform
    public GameObject pingu;
    public Camera cam;
    public int distanceBetweenPoints = 5;
    public int heightOffset = 20;
    public int curvature = 30;
    public float generationThreshold = 50f;  // Distance from the edge of the terrain to trigger new generation
    public float deletionThreshold = 50f;  // Increased distance behind the character to delete points
    public float shiftVelocity = -4f;

    private float lowestPointYPos;
    private int lastPointIndex = 4;
    private readonly int firstPointIndex = 1;
    private SpriteShapeController terrainShape;
    private Rigidbody2D terrainRigidBody;
    private EdgeCollider2D edgeCollider;

    void Start()
    {
        terrainShape = GetComponent<SpriteShapeController>();
        terrainRigidBody = GetComponent<Rigidbody2D>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        lowestPointYPos = terrainShape.spline.GetPosition(0).y;
        if (cam == null)
        {
            cam = Camera.main;  // Fallback to main camera if not assigned
        }
    }

    void Update()
    {
        rigidBody2D.velocity = new Vector2(shiftVelocity, 0f);
        if (cam != null && terrainShape != null)
        {
            // Calculate camera's left and right edges in world space
            float cameraLeftEdgeX = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane)).x;
            float cameraRightEdgeX = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane)).x;

            // Check if new points need to be generated
            float lastPointXPos = terrainShape.transform.position.x + terrainShape.spline.GetPosition(lastPointIndex).x;
            if (cameraRightEdgeX > lastPointXPos - generationThreshold) // Don't ask me why I divide by four its the only way it works XD
            {
                GenerateNewPoint();
            }

            // Check if old points need to be deleted
            float secondPointXpos = terrainShape.transform.position.x + terrainShape.spline.GetPosition(firstPointIndex+1).x;
            if (cameraLeftEdgeX > secondPointXpos + deletionThreshold)
            {
                DeleteOldPoint();
            }
        }
    }

    void GenerateNewPoint()
    {
        // Generate new point positions using Perlin noise
        float xPos = terrainShape.spline.GetPosition(lastPointIndex).x + distanceBetweenPoints;
        float yPos = heightOffset * Mathf.PerlinNoise(lastPointIndex * UnityEngine.Random.Range(5.0f, 15.0f), 0);

        // Insert point
        terrainShape.spline.InsertPointAt(lastPointIndex + 1, new Vector3(xPos, yPos, 0));
        terrainShape.spline.SetTangentMode(lastPointIndex + 1, ShapeTangentMode.Continuous);
        terrainShape.spline.SetLeftTangent(lastPointIndex + 1, new Vector3(-curvature / 10, 0, 0));
        terrainShape.spline.SetRightTangent(lastPointIndex + 1, new Vector3(curvature / 10, 0, 0));
        lastPointIndex++;

        // Update bottom right point to keep shape
        int bottomRightPointIndex = terrainShape.spline.GetPointCount() - 1;
        terrainShape.spline.SetPosition(bottomRightPointIndex, new Vector3(xPos, terrainShape.spline.GetPosition(bottomRightPointIndex).y, 0));
    }

    void DeleteOldPoint()
    {
        if (terrainShape.spline.GetPointCount() > 4 && firstPointIndex < lastPointIndex)  // Ensure we have enough points to delete
        {
            // Remove the leftmost upper point
            terrainShape.spline.RemovePointAt(firstPointIndex);

            // Update the bottom left point to keep it aligned with the new first upper point
            int bottomLeftPointIndex = 0;
            float newXPos = terrainShape.spline.GetPosition(firstPointIndex).x;  // Access the new first point after deletion
            terrainShape.spline.SetPosition(bottomLeftPointIndex, new Vector3(newXPos, lowestPointYPos, 0));

            // Adjust the indices to account for the deletion
            lastPointIndex--;

            // Ensure the x position of the shape is the same as the left most point
            AdjustShapePosition();
        }
    }

    void AdjustShapePosition()
    {
        float offsetX = terrainShape.spline.GetPosition(firstPointIndex).x;

        Spline spline = terrainShape.spline;

        // Shift shape right
        terrainShape.transform.Translate(offsetX, 0, 0);

        //Shift all points left
        for (int i = 0; i < spline.GetPointCount(); i++)
        {
            spline.SetPosition(i, spline.GetPosition(i) - new Vector3(offsetX, 0, 0));
        }
    }

}
