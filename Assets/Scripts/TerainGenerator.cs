using UnityEngine;
using UnityEngine.U2D;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

public class TerainGenerator : MonoBehaviour
{
    public Transform character;  // Reference to the character's transform
    public GameObject pingu;
    public Camera cam;
    public int distanceBetweenPoints = 5;
    public int heightOffset = 20;
    public int curvature = 30;
    public float generationThreshold = 50f;  // Distance from the edge of the terrain to trigger new generation
    public float deletionThreshold = 50f;  // Increased distance behind the character to delete points
    public float waveSpread = 1f;
    //public float waveResistance = 5f;
    public float waveStiffness = 0.05f;
    public float waveDampening = 0.99999999999f;
    public float waveMinHeight = 0.5f;
    public float waveMaxHeight = 1f;
    public float waveFrequency = 5;
    public int waveFrequencyVariationPercentage = 50;

    private float lowestPointYPos;
    private int lastPointIndex = 21;
    private readonly int firstPointIndex = 1;
    private SpriteShapeController terrainShape;
    private Rigidbody2D terrainRigidBody;
    private Rigidbody2D pinguRigidBody;
    private EdgeCollider2D edgeCollider;
    private List<float> velocities = new();
    private List<float> target_heights = new();

    private float waveTimer = 0;
    private float currentWaveFrequency;

    void Start()
    {
        terrainShape = GetComponent<SpriteShapeController>();
        terrainRigidBody = GetComponent<Rigidbody2D>();
        pinguRigidBody = pingu.GetComponent<Rigidbody2D>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        lowestPointYPos = terrainShape.spline.GetPosition(0).y;
        if (cam == null)
        {
            cam = Camera.main;  // Fallback to main camera if not assigned
        }

        // Initialize water physics
        for(int i = 0; i < terrainShape.spline.GetPointCount(); i++)
        {
            velocities.Add(0f);
            target_heights.Add(terrainShape.spline.GetPosition(i).y);
        }

        Splash(15, waveMaxHeight);

        currentWaveFrequency = waveFrequency;
    }

    void Update()
    {
        waveTimer += Time.deltaTime;
        if (waveTimer > currentWaveFrequency)
        {
            GenerateWave(UnityEngine.Random.Range(waveMinHeight, waveMaxHeight));
            waveTimer = 0;
            currentWaveFrequency = UnityEngine.Random.Range(waveFrequency * 0.01f * (100 - waveFrequencyVariationPercentage), waveFrequency * 0.01f * (100 + waveFrequencyVariationPercentage));
        }

        if (cam != null && terrainShape != null)
        {
            // Calculate camera's left and right edges in world space
            float cameraLeftEdgeX = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane)).x;
            float cameraRightEdgeX = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane)).x;

            // Check if new points need to be generated
            float lastPointXPos = terrainShape.transform.position.x + terrainShape.spline.GetPosition(lastPointIndex).x;
            if (cameraRightEdgeX > lastPointXPos - generationThreshold)
            {
                GenerateNewPoint();
            }

            // Check if old points need to be deleted
            float secondPointXpos = terrainShape.transform.position.x + terrainShape.spline.GetPosition(firstPointIndex+1).x;
            if (cameraLeftEdgeX > secondPointXpos + deletionThreshold)
            {
                DeleteOldPoint();
            }

            for (int i = firstPointIndex; i < lastPointIndex; i++)
            {
                if (i < velocities.Count && i < target_heights.Count)
                {
                    WaveSpringUpdate(i, waveStiffness, waveDampening, Time.deltaTime);
                    Smoothen(i);
                }
            }

            PropagateWaves(Time.deltaTime);
        }
    }

    void GenerateNewPoint()
    {
        // Generate new point positions using Perlin noise
        float xPos = terrainShape.spline.GetPosition(lastPointIndex).x + distanceBetweenPoints;
        float yPos = heightOffset * Mathf.PerlinNoise(lastPointIndex * UnityEngine.Random.Range(5.0f, 15.0f), 0);

        // Insert point
        terrainShape.spline.InsertPointAt(lastPointIndex + 1, new Vector3(xPos, 20, 0));
        terrainShape.spline.SetTangentMode(lastPointIndex + 1, ShapeTangentMode.Continuous);
        terrainShape.spline.SetLeftTangent(lastPointIndex + 1, new Vector3(-curvature / 10, 0, 0));
        terrainShape.spline.SetRightTangent(lastPointIndex + 1, new Vector3(curvature / 10, 0, 0));
        lastPointIndex++;

        // Update bottom right point to keep shape
        int bottomRightPointIndex = terrainShape.spline.GetPointCount() - 1;
        terrainShape.spline.SetPosition(bottomRightPointIndex, new Vector3(xPos, terrainShape.spline.GetPosition(bottomRightPointIndex).y, 0));

        // Set initial velocity and target height
        velocities.Insert(lastPointIndex, 0f);
        target_heights.Insert(lastPointIndex, terrainShape.spline.GetPosition(lastPointIndex).y);

        // Smoothen
        //Smoothen(lastPointIndex - 1);
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

            // Remove velocity and target height
            velocities.RemoveAt(firstPointIndex);
            target_heights.RemoveAt(firstPointIndex);
        }
    }

    void GenerateWave(float height)
    {
        Debug.Log("WAVE!");
        Splash(lastPointIndex, height);
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

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("COLLISION!");
    //    if (collision.gameObject.tag.Equals("Pingu"))
    //    {
    //        Tuple<int, float> indexDistance = GetClosestPointIndexAndDistance();
    //        velocities[indexDistance.Item1] += (indexDistance.Item2 / distanceBetweenPoints) * (pinguRigidBody.velocity.y / waveResistance);
    //    }
    //}

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag.Equals("Pingu"))
    //    {
    //        Rigidbody2D objectRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
    //        if (objectRigidbody != null)
    //        {
    //            // Get object's current position
    //            Vector3 objectPosition = collision.transform.position;

    //            // Calculate the corresponding x position in the spline
    //            Tuple<int, float> indexDistance = GetClosestPointIndexAndDistance();
    //            int closestPointIndex = indexDistance.Item1;

    //            // Get the y position of the terrain at this point
    //            float terrainY = terrainShape.spline.GetPosition(closestPointIndex).y;

    //            // Calculate depth
    //            float depth = terrainY - objectPosition.y;

    //            if (depth > 0)
    //            {
    //                // Apply a buoyancy force proportional to depth
    //                float buoyancyForce = depth * 10f;  // Adjust the multiplier to tune buoyancy strength
    //                objectRigidbody.AddForce(Vector2.up * buoyancyForce, ForceMode2D.Force);
    //            }
    //        }
    //    }
    //}

    private Tuple<int, float> GetClosestPointIndexAndDistance()
    {
        float minDistance = 1000f;
        int index = firstPointIndex;
        for (int i = firstPointIndex; i < lastPointIndex; i++)
        {
            float distance = Math.Abs(character.position.x - (terrainShape.transform.position.x + terrainShape.spline.GetPosition(i).x));
            if (distance < minDistance)
            {
                minDistance = distance;
                index = i;
            }
        }
        return new Tuple<int, float>(index, minDistance);
    }

    void WaveSpringUpdate(int index, float springStiffness, float dampening, float deltaTime)
    {
        float height = terrainShape.spline.GetPosition(index).y;

        // max extension
        float x = height - target_heights[index];
        float loss = -dampening * velocities[index];

        float force = 0;
        if (Math.Abs(x) > 0.0001f)
        {
            force = -springStiffness * x + loss;
        }
        velocities[index] += force * deltaTime;
        terrainShape.spline.SetPosition(index, new Vector3(terrainShape.spline.GetPosition(index).x, height + velocities[index], terrainShape.spline.GetPosition(index).z));
    }

    void PropagateWaves(float deltaTime)
    {
        int count = terrainShape.spline.GetPointCount();
        float[] left_deltas = new float[count];
        float[] right_deltas = new float[count];
        for(int i = 0; i < count; i++)
        {
            if (i > 1)
            {
                left_deltas[i] = waveSpread * ((terrainShape.spline.GetPosition(i).y - terrainShape.spline.GetPosition(i-1).y) - (target_heights[i] - target_heights[i-1]));
                velocities[i - 1] += left_deltas[i] * deltaTime;
            }
            if (i < terrainShape.spline.GetPointCount() - 2)
            {
                right_deltas[i] = waveSpread * ((terrainShape.spline.GetPosition(i).y - terrainShape.spline.GetPosition(i+1).y) - (target_heights[i] - target_heights[i+1]));
                velocities[i + 1] += right_deltas[i] * deltaTime;
            }
        }
    }

    void Splash(int index, float speed)
    {
        if (index >= 0 && index < terrainShape.spline.GetPointCount())
        {
            velocities[index] += speed;
        }
    }

    void Smoothen(int index)
    {
        Spline waterSpline = terrainShape.spline;
        Vector3 position = waterSpline.GetPosition(index);
        Vector3 positionPrev = position;
        Vector3 positionNext = position;

        if (index >= firstPointIndex + 1)
        {
            positionPrev = waterSpline.GetPosition(index-1);
        }
        if (index <= lastPointIndex - 1)
        {
            positionNext = waterSpline.GetPosition(index+1);
        }
        Vector3 forward = gameObject.transform.forward;

        float scale = Mathf.Min((positionNext - position).magnitude, (positionPrev - position).magnitude) * 0.33f;
        Vector3 leftTangent = (positionPrev - position).normalized * scale;
        Vector3 rightTangent = (positionNext - position).normalized * scale;
        SplineUtility.CalculateTangents(position, positionPrev, positionNext, forward, scale, out rightTangent, out leftTangent);

        waterSpline.SetLeftTangent(index, leftTangent);
        waterSpline.SetRightTangent(index, rightTangent);
    }
}
