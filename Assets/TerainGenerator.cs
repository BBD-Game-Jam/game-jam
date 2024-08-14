using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TerainGenerator : MonoBehaviour
{
    public SpriteShapeController shape;
    public int scale = 100;
    public int numPoints = 150;
    public int distanceBetweenPoints = 3;
    public int heightOffset = 10;
    public int curvature = 5;

    // Start is called before the first frame update
    void Start()
    {
        shape = GetComponent<SpriteShapeController>();

        shape.spline.SetPosition(0, shape.spline.GetPosition(0) + 10 * Vector3.left);
        shape.spline.SetPosition(1, shape.spline.GetPosition(1) + 10 * Vector3.left);

        shape.spline.SetPosition(2, shape.spline.GetPosition(2) + numPoints * distanceBetweenPoints * Vector3.right);
        shape.spline.SetPosition(3, shape.spline.GetPosition(3) + numPoints * distanceBetweenPoints * Vector3.right );

        for (int i = 0; i < numPoints; i++)
        {
            float xPos = shape.spline.GetPosition(i + 1).x + distanceBetweenPoints;
            shape.spline.InsertPointAt(i + 2, new Vector3(xPos, heightOffset * Mathf.PerlinNoise(i * Random.Range(5.0f, 15.0f), 0), 0));
        }

        for (int i = 2;  i < numPoints + 2; i++)
        {
            shape.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
            shape.spline.SetLeftTangent(i, new Vector3(-curvature / 10, 0, 0));
            shape.spline.SetRightTangent(i, new Vector3(curvature / 10, 0, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
