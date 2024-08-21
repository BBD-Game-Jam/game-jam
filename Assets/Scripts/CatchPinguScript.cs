using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchPinguScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pingu;

    public GameObject gameOverUI;

    public float enemySpeed = 1f;

    public int upperLimit = 5;
    public bool pausedFlag = false;
    public float difficulty = 2f;

    public int followDistance = 100;

    public float appearanceDistance;

    void Start()
    {
        StartCoroutine(ChangeValueRoutine());
        initializePosition();
    }

    IEnumerator ChangeValueRoutine()
    {
        int limitCount = 0;
        while (true && !pausedFlag)
        {
            yield return new WaitForSeconds(10);
            if (limitCount <= upperLimit)
            {
                // enemySpeed *= difficulty;
                //Debug.Log("Enemy speed before change: " + enemySpeed);
                //Debug.Log("Difficulty after change: " + difficulty);
                enemySpeed = enemySpeed * difficulty;
                //Debug.Log("Enemy speed after change: " + enemySpeed);
                limitCount++;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * (enemySpeed * Time.deltaTime);
        if ((pingu.transform.position.x - transform.position.x) > followDistance)
        {
            transform.position = new Vector3(pingu.transform.position.x - followDistance, pingu.transform.position.y, transform.position.z);

        }
        transform.position = new Vector3(transform.position.x, pingu.transform.position.y, transform.position.z);
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
        transform.position = new Vector3(pingu.transform.position.x - 20f, pingu.transform.position.y, pingu.transform.position.z);
    }
}
