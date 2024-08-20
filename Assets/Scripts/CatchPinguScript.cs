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

    public float enemySpeed = 15f;

    public int upperLimit = 5;
    public bool pausedFlag = false;
    public float difficulty = 1.25f;

    private float enemyBaseSpeed;
    private bool gameOverFlag = false;

    void Start()
    {
        enemyRigidBody = gameObject.GetComponent<Rigidbody2D>();
        enemyRigidBody.isKinematic = true;
        StartCoroutine(ChangeValueRoutine());
        initializePosition();
        enemyBaseSpeed = enemySpeed;
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
                enemyBaseSpeed *= difficulty;
                limitCount++;
            }
        }
    }

    void Update()
    {
        enemySpeed = enemyBaseSpeed;
        Vector3 predictedPosition = pingu.transform.position + new Vector3(pingu.GetComponent<Rigidbody2D>().velocity.x, pingu.GetComponent<Rigidbody2D>().velocity.y, 0);
        float distanceToPlayer = Vector3.Distance(transform.position, predictedPosition);
        if (distanceToPlayer > 200f)
        {
            enemySpeed = 60f;
        }
        if (gameOverFlag)
        {
            enemyRigidBody.velocity = new Vector2(pingu.GetComponent<Rigidbody2D>().velocity.x, pingu.GetComponent<Rigidbody2D>().velocity.y);
        }
        else
        {
            enemyRigidBody.velocity = new Vector2(enemySpeed, pingu.GetComponent<Rigidbody2D>().velocity.y);
        }

        gameOver();

    }
    public void gameOver()
    {
        if (pingu.transform.position.x <= gameObject.transform.position.x)
        {
            gameOverFlag = true;
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
