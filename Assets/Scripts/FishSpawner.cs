using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public float minSpawnDistance = 27f;
    public float maxSpawnDistance = 150f;
    public float spawnRate = 0;
    public float generationThresholdX = 20f;
    public Camera cam;
    public GameObject fish;
    private RaycastHit2D hitSurface;
    private RaycastHit2D hitSeabed;

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
            SpawnFish();
            lastSpawnXPosition = currentSpawnXPosition; // Update last spawn position

        }
        else if ((currentSpawnXPosition - lastSpawnXPosition) < 0)
        {
            lastSpawnXPosition = currentSpawnXPosition;
            // Debug.Log("ResetSpawn");
        }

        // Debug.Log($"Diff: {currentSpawnXPosition - lastSpawnXPosition}");
    }

    [ContextMenu("Spawn fish")]
    public void SpawnFish()
    {
        float x = currentSpawnXPosition;
        float y;
        
        hitSurface = Physics2D.Raycast(new Vector2(x, 55f), Vector2.down);
        hitSeabed = Physics2D.Raycast(new Vector2(x, hitSurface.point.y - 6f), Vector2.down);

        if ((hitSurface != null) && (hitSeabed.collider != null))
        {
            
            y = Random.Range(hitSeabed.point.y + 2f, hitSurface.point.y - 2f);
            Instantiate(fish, new Vector3(x, y, 0), transform.rotation);
            // Debug.Log($"Height: {y}");
        }
        spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
    }
}
