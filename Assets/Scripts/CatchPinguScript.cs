using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;


public class CatchPinguScript : MonoBehaviour
{
  // Start is called before the first frame update
  public GameObject pingu;
  public SpriteShapeController terrainShape;

    public GameObject gameOverUI;
    private Rigidbody2D enemyRigidBody;
    public AudioClip gameOverAudioClip;
    private Vector3 spinePos;


  public float enemySpeed = 15f;

  public int upperLimit = 5;
  public bool pausedFlag = false;
  public float difficulty = 1.25f;

  private float enemyBaseSpeed;
  private bool gameOverFlag = false;
  private float yWave = 0;
  public float yWaveMagnitude = 1f;
  public float yWaveSpeed = 1f;
  private float futureSpinePos = 1f;

  void Start()
  {
    enemyRigidBody = gameObject.GetComponent<Rigidbody2D>();
    enemyRigidBody.isKinematic = true;
    StartCoroutine(ChangeValueRoutine());
    initializePosition();
    enemyBaseSpeed = enemySpeed;
    // StartCoroutine(AverageSpeedRoutine());
    StartCoroutine(WaveRideCoroutine());

    var gameOverAudioSource = GameObject.FindGameObjectWithTag("GameOverAudio").GetComponent<AudioSource>();
    gameOverAudioClip = gameOverAudioSource.clip;
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

  IEnumerator WaveRideCoroutine()
  {
    while (true)
    {
      yield return new WaitForSeconds(2);
      spinePos = GetPositionOnSpline(enemyRigidBody.position.x + 30f);

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


    float angle = Mathf.Atan2(enemyRigidBody.velocity.y, enemyRigidBody.velocity.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(0, 0, angle);
    yWave = spinePos.y / 2 * yWaveSpeed * Mathf.Cos(Time.time * yWaveSpeed);
   // Debug.Log($"yWave: {yWave}");

    if (gameOverFlag)
    {
      // enemyRigidBody.velocity = new Vector2(pingu.GetComponent<Rigidbody2D>().velocity.x, pingu.GetComponent<Rigidbody2D>().velocity.y); // Straight line follow
      enemyRigidBody.velocity = new Vector2(pingu.GetComponent<Rigidbody2D>().velocity.x, yWave); // Wave follow
    }
    else
    {
      // enemyRigidBody.velocity = new Vector2(enemySpeed, pingu.GetComponent<Rigidbody2D>().velocity.y);  // Straight line follow
      enemyRigidBody.velocity = new Vector2(enemySpeed, yWave); // Wave follow
    }

    gameOver();

  }
  public void gameOver()
  {
    if (pingu.transform.position.x <= gameObject.transform.position.x)
    {
      gameOverFlag = true;
      gameOverUI.SetActive(true);
      Time.timeScale = 0f;
    }
  }

  void initializePosition()
  {
    transform.position = new Vector3(pingu.transform.position.x - 70f, pingu.transform.position.y - 15f, pingu.transform.position.z);
  }

  Vector3 GetPositionOnSpline(float distance)
  {
    int pointCount = terrainShape.spline.GetPointCount();
    Debug.Log($"point count : {pointCount}");

    for (int i = 0; i < pointCount - 1; i++)
    {
      Vector3 p1 = terrainShape.spline.GetPosition(i);
      Vector3 p2 = terrainShape.spline.GetPosition(i + 1);
      float segmentLength = Vector3.Distance(p1, p2);

      if (distance <= segmentLength)
      {
        float t = distance / segmentLength;
        return Vector3.Lerp(p1, p2, t);
      }
      else
      {
        distance -= segmentLength;
      }
    }
    return terrainShape.spline.GetPosition(pointCount - 1);
  }
}
