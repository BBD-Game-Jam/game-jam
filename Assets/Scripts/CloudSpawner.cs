using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{

    // Start is called before the first frame update
    public float minSpawnDistance = 10f;
    public float maxSpawnDistance = 35f;
    public float spawnRate = 0;
    public float spawnWidthSeparation = 2.2f;
    public float spawnHeightSeparation = 2.2f;
    //public float speedIncrease = 0f;
    public float generationThresholdX = 20f;
    public float generationThresholdY = 75f;
    public float maxSpawnHeightDensity = 0.25f;
    //public float cloudHeight = 4.5f;
    public float minHeight = 30f;
    public Camera cam;
    public GameObject clouds;

    private float lastSpawnXPosition;
    private float currentSpawnXPosition;
    private float spawnDistance;
    // Start is called before the first frame update
    void Start()
    {
        spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
        lastSpawnXPosition = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane)).x + generationThresholdX;
        SpawnClouds();
    }

    // Update is called once per frame
    void Update()
    {
        
        currentSpawnXPosition = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane)).x + generationThresholdX;

        // Check if the player has moved the spawnDistance since the last spawn
        if (currentSpawnXPosition - lastSpawnXPosition >= spawnDistance)
        {
            SpawnClouds();
            lastSpawnXPosition = currentSpawnXPosition; // Update last spawn position
            
        }
        else if ((currentSpawnXPosition - lastSpawnXPosition) < 0)
        {
            lastSpawnXPosition = currentSpawnXPosition;
            // Debug.Log("ResetSpawn");
        }

       // Debug.Log($"Diff: {currentSpawnXPosition - lastSpawnXPosition}");
    }

    [ContextMenu("Spawn Clouds")]
    public void SpawnClouds()
    {
        int generateOrNot = (int)Random.Range(0f, spawnRate);
        float spawnHeightDensity = Random.Range(0.0001f, maxSpawnHeightDensity);
        float maxSpawnHeight = cam.ViewportToWorldPoint(new Vector3(0, 1, cam.nearClipPlane)).y + generationThresholdY;
        int numClouds = (int)((maxSpawnHeight / spawnHeightSeparation * spawnHeightDensity));
        //Debug.Log($"MaxSpH: {maxSpawnHeight}\nSpHSep: {spawnHeightSeparation}\nSpHDen: {spawnHeightDensity}\nclouds: {numClouds}");

        for (int i = 0; i < numClouds; i++)
        {
            if (generateOrNot == 0)
            {
                float x = currentSpawnXPosition;
                float y = Random.Range(minHeight, maxSpawnHeight);
                Instantiate(clouds, new Vector3(x, y, 0), transform.rotation);
            }
        }
        spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
    }
}
