using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TerainGenerator : MonoBehaviour
{
    public SpriteShapeController shape;
    public int distanceBetweenPoints = 3;
    public int heightOffset = 15;
    public int curvature = 25;
    public Transform character;  // Reference to the character's transform
    public float generationThreshold = 50f;  // Distance from the edge of the terrain to trigger new generation
    public float deletionThreshold = 100f;  // Distance behind the character to delete points

    private float lowestPointYPos;
    private int lastPointIndex = 3;
    private readonly int firstPointIndex = 1;

    void Start()
    {
        shape = GetComponent<SpriteShapeController>();
        lowestPointYPos = shape.spline.GetPosition(0).y;
    }

    void Update()
    {
        if (character != null && shape != null)
        {
            float lastPointXPos = shape.spline.GetPosition(lastPointIndex).x;
            if (character.position.x > lastPointXPos - generationThreshold)
            {
                GenerateNewPoint();
            }

            float firstPointXPos = shape.spline.GetPosition(firstPointIndex).x;
            if (character.position.x > firstPointXPos + deletionThreshold)
            {
                DeleteOldPoint();
            }
        }
    }

    void GenerateNewPoint()
    {
        // Generat new point positions using Perlin noise
        float xPos = shape.spline.GetPosition(lastPointIndex).x + distanceBetweenPoints;
        float yPos = heightOffset * Mathf.PerlinNoise(lastPointIndex * Random.Range(5.0f, 15.0f), 0);

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

            // Decrement lastPointIndex as we've removed one point from the spline
            lastPointIndex--;

            // No need to increment firstPointIndex, as it's always pointing to the first upper point after the deletion
        }
    }
}
