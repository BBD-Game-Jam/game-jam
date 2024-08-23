using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
  // Start is called before the first frame update
  public Collider2D terrain;
  public float minSpawnDistance = 10f;
  public float maxSpawnDistance = 35f;
  public float spawnRate = 0;
  public float spawnWidthSeparation = 2.2f;
  public float spawnHeightSeparation = 2.2f;
  //public float speedIncrease = 0f;
  public float generationThresholdX = 20f;
  public float generationThresholdY = 75f;
  public float maxSpawnHeightDensity = 0.025f;
  //public float coinHeight = 2f;
  public float minHeight = 30f;
  public Camera cam;
  public GameObject coins;

  private float lastSpawnXPosition;
  private float currentSpawnXPosition;
  private float spawnDistance;


  void Start()
  {
    spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
    lastSpawnXPosition = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane)).x + generationThresholdX;
    SpawnCoins();
  }

  // Update is called once per frame
  void Update()
  {
    currentSpawnXPosition = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane)).x + generationThresholdX;

    // Check if the player has moved the spawnDistance since the last spawn
    if (currentSpawnXPosition - lastSpawnXPosition >= spawnDistance)
    {
      SpawnCoins();
      lastSpawnXPosition = currentSpawnXPosition; // Update last spawn position
    }
    else if ((currentSpawnXPosition - lastSpawnXPosition) < 0)
    {
      lastSpawnXPosition = currentSpawnXPosition;
    }
  }

  [ContextMenu("Spawn Coins")]
  public void SpawnCoins()
  {
    int generateOrNot = (int)Random.Range(0f, spawnRate);
    float spawnHeightDensity = Random.Range(0.0001f, maxSpawnHeightDensity);
    float maxSpawnHeight = cam.ViewportToWorldPoint(new Vector3(0, 1, cam.nearClipPlane)).y + generationThresholdY;
    int numCoins = (int)((maxSpawnHeight / spawnHeightSeparation * spawnHeightDensity));
    // Debug.Log($"CamH: {camHeight}\nGenY: {generationThresholdY}\nSpHSep: {spawnHeightSeparation}\nSpHDen: {spawnHeightDensity}\ncoins: {numCoins}");

    for (int i = 0; i < numCoins; i++)
    {
      if (generateOrNot == 0)
      {
        float x = currentSpawnXPosition;
        float y = Random.Range(minHeight / spawnHeightSeparation, maxSpawnHeight / spawnHeightSeparation) * spawnHeightSeparation;
        // Vector3 spawnPosition = new Vector3(x, y, 0);

        // Check if the position overlaps with terrain
        // Collider2D overlapCheck = Physics2D.OverlapCircle(spawnPosition, coinRadius, terrainLayerMask);

        GameObject coinsObj = Instantiate(coins, new Vector3(x, y, 0), transform.rotation);

        /*if (terrain.IsTouching(coinsObj.GetComponent<Collider2D>()))
        {
            Destroy(coinsObj);
        }*/
      }
    }
    spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
  }
}
