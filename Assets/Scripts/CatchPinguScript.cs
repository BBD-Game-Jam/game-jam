using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CatchPinguScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pingu;

    public GameObject gameOverUI;
    private Rigidbody2D enemyRigidBody;

    public float enemySpeed = 20f;

    public int upperLimit = 5;
    public bool pausedFlag = false;
    public float difficulty = 2f;

    public int followDistance = 100;

    public float appearanceDistance;
    public float baseSpeed = 10f; // Base speed for the enemy
    private float maxSpeed; // Maximum speed for the enemy
    public float acceleration = 5f; // How quickly the enemy speeds up

    private List<float> msList = new List<float>();
    private float pinguVelocityVecX;

    void Start()
    {
        enemyRigidBody = gameObject.GetComponent<Rigidbody2D>();
        enemyRigidBody.isKinematic = true;
        StartCoroutine(ChangeValueRoutine());
        initializePosition();
        maxSpeed = 0;
        // StartCoroutine(AverageSpeedRoutine());

    }

    IEnumerator ChangeValueRoutine()
    {
        int limitCount = 0;
        while (true && !pausedFlag)
        {
            yield return new WaitForSeconds(10);
            if (limitCount <= upperLimit)
            {
                enemySpeed *= difficulty;
                limitCount++;
            }
        }
    }

    void Update()
    {
        enemySpeed = 25f;
        Vector3 predictedPosition = pingu.transform.position + new Vector3(pingu.GetComponent<Rigidbody2D>().velocity.x, pingu.GetComponent<Rigidbody2D>().velocity.y, 0);
        float distanceToPlayer = Vector3.Distance(transform.position, predictedPosition);
        if (distanceToPlayer > 200)
        {
            enemySpeed = 50f;
        }

        enemyRigidBody.velocity = new Vector2(enemySpeed, enemyRigidBody.velocity.y);

        // float playerSpeed = pingu.GetComponent<Rigidbody2D>().velocity.x;
        // // if (maxSpeed < playerSpeed)
        // // {
        // //     maxSpeed = playerSpeed + 2f;
        // // }
        // maxSpeed = playerSpeed;

        // Vector3 predictedPosition = pingu.transform.position + new Vector3(pingu.GetComponent<Rigidbody2D>().velocity.x, pingu.GetComponent<Rigidbody2D>().velocity.y, 0);
        // float distanceToPlayer = Vector3.Distance(transform.position, predictedPosition);

        // if (distanceToPlayer > 200f)
        // {
        //     Debug.Log("Catch up mode");
        //     enemyRigidBody.velocity = new Vector2(Mathf.Min(maxSpeed, enemyRigidBody.velocity.x + acceleration), enemyRigidBody.velocity.y);
        // }
        // else if (distanceToPlayer < 50f)
        // {
        //     Debug.Log("Slow down mode");
        //     enemyRigidBody.velocity = new Vector2(Mathf.Max(baseSpeed, enemyRigidBody.velocity.x - acceleration), enemyRigidBody.velocity.y);
        // }
        // else
        // {
        //     Debug.Log("Neutral mode");
        //     enemyRigidBody.velocity = new Vector2(Mathf.Clamp(enemyRigidBody.velocity.x + acceleration, baseSpeed, maxSpeed), enemyRigidBody.velocity.y);
        // }


        gameOver();

    }
    public void gameOver()
    {
        if (pingu.transform.position.x <= gameObject.transform.position.x)
        {
            // Debug.Log("Game over at " + transform.position.x);
            gameOverUI.SetActive(true);
        }
    }

    void initializePosition()
    {
        //Sets the position of the enemy to the left of the pengu at start
        transform.position = new Vector3(pingu.transform.position.x - 70f, pingu.transform.position.y, pingu.transform.position.z);
    }
}
