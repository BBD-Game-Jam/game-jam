using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCoins : MonoBehaviour
{
  public Camera cam;
  public float deletionThreshold = 200f;
    private RaycastHit2D hit;
    // Start is called before the first frame update
    void Start()
  {
    cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
  }

  // Update is called once per frame
  void Update()
  {
    float cameraLeftEdgeX = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane)).x;

    if (transform.position.x < (cameraLeftEdgeX - deletionThreshold))
    {
      Destroy(gameObject);
    }

        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;
        hit = Physics2D.Raycast(new Vector2(x, y - 0.5f), Vector2.down);
        if (hit.collider != null)
        {
            y = hit.point.y;
            gameObject.transform.position = new Vector2(x, y + 0.5f);
        }
    }
}
