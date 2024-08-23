using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
  // Start is called before the first frame update
  public float minSpawnDistance = 310f;
  public float maxSpawnDistance = 950f;
  public float generationThresholdX = 20f;
  public float distanceToRightOfWavePeak = 1f;
  public float distanceAboveWavePeak = 7f;
  public float waveHeight = 7f;
  public Camera cam;
  public GameObject powerup;
  private RaycastHit2D hit;

  private float lastSpawnXPosition;
  private float currentSpawnXPosition;
  private float spawnDistance;
  private float hp;
  private float lp;
  // Start is called before the first frame update

  void Start()
  {
    hp = -9999;
    lp = 9999;
    spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
    lastSpawnXPosition = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane)).x + generationThresholdX;
  }

  // Update is called once per frame
  void Update()
  {
    currentSpawnXPosition = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane)).x + generationThresholdX;
    float cp;
    // Check if the player has moved the spawnDistance since the last spawn
    if (currentSpawnXPosition - lastSpawnXPosition >= spawnDistance)
    {

      hit = Physics2D.Raycast(new Vector2(currentSpawnXPosition, 70f), Vector2.down);
      cp = hit.point.y;
      if (cp < lp)
      {
        lp = cp;
      }
      else if (cp > hp)
      {
        hp = cp;
      }
      else if ((hp - lp) > waveHeight)
      {
        generatePowerup(currentSpawnXPosition, cp);
        hp = -9999;
        lp = 9999;
      }

      lastSpawnXPosition = currentSpawnXPosition; // Update last spawn position
    }
    else if ((currentSpawnXPosition - lastSpawnXPosition) < 0)
    {
      lastSpawnXPosition = currentSpawnXPosition;
      /*Debug.Log("ResetSpawn");*/
    }
    // Debug.Log($"Diff: {currentSpawnXPosition - lastSpawnXPosition}");
  }

  [ContextMenu("Spawn floor coins")]
  public void generatePowerup(float x, float y)
  {
    Debug.Log("Generate powerup");
    Instantiate(powerup, new Vector3(x, y + distanceAboveWavePeak, 0), transform.rotation);
    spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
  }
}
