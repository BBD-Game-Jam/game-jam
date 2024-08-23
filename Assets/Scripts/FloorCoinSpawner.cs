using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class FloorCoinSpawner : MonoBehaviour
{

    // Start is called before the first frame update
    public float minSpawnDistance = 15f;
    public float maxSpawnDistance = 50f;
    public float spawnRate = 0;
    public float coinSeparation = 3f;
    public float generationThresholdX = 20f;
    public float distanceFromFloor = 1f;
    public float maxCoinSet = 6f;
    public Camera cam;
    public GameObject coin;
    private RaycastHit2D hit;

    private float lastSpawnXPosition;
    private float currentSpawnXPosition;
    private float spawnDistance;
    // Start is called before the first frame update

    void Start()
    {
        spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
        lastSpawnXPosition = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane)).x + generationThresholdX;
    }

    // Update is called once per frame
    void Update()
    {
        currentSpawnXPosition = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane)).x + generationThresholdX;
       
        // Check if the player has moved the spawnDistance since the last spawn
        if (currentSpawnXPosition - lastSpawnXPosition >= spawnDistance)
        {
            int numCoins = (int) Random.Range(1,maxCoinSet);
            SpawnFloorCoins(numCoins);
            lastSpawnXPosition = currentSpawnXPosition; // Update last spawn position

        }
        else if ((currentSpawnXPosition - lastSpawnXPosition) < 0)
        {
            lastSpawnXPosition = currentSpawnXPosition;
            Debug.Log("ResetSpawn");
        }

        // Debug.Log($"Diff: {currentSpawnXPosition - lastSpawnXPosition}");
    }

    [ContextMenu("Spawn floor coins")]
    public void SpawnFloorCoins(int numCoins)
    {
        float x = currentSpawnXPosition;
        for (int i = 0; i < numCoins; i++)
        {
            float y = 50f;
            hit = Physics2D.Raycast(new Vector2(x, 70f), Vector2.down);
            if (hit.collider != null)
            {
                y = hit.point.y;
                //Debug.Log($"Height: {y}");
            }

            Instantiate(coin, new Vector3(x, y + distanceFromFloor, 0), transform.rotation);
            x = x + coinSeparation;
        }

        spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
    }
}
