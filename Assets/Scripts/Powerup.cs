using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
  // Start is called before the first frame update
  public LogicScript logic;
    private RaycastHit2D hit;
    private AudioClip powerUpClip;

    void Start()
  {
    logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        var audioSource = GameObject.FindGameObjectWithTag("PowerupCollectedAudio").GetComponent<AudioSource>();
        powerUpClip = audioSource.clip;
    }

  // Update is called once per frame
  void Update()
  {
        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;
        int layerToIgnore = 6;
        int layerMask = ~(1 << layerToIgnore);

        // Perform a raycast downwards, ignoring the specified layer
        hit = Physics2D.Raycast(new Vector2(x, y), Vector2.down, Mathf.Infinity, layerMask);
        //hit = Physics2D.Raycast(new Vector2(x, y - 0.5f), Vector2.down);
        if (hit.collider != null)
        {
            //Debug.Log(y);
            y = hit.point.y;
            gameObject.transform.position = new Vector2(x, y + 5f);
        }

    }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    int type = (int)Mathf.Round(Random.Range(0, 2));
    if (collision.gameObject.layer == 3)
    {
            AudioSource.PlayClipAtPoint(powerUpClip, transform.position);
            //Debug.Log("Collided with power");
      logic.Powerup(type);
            Destroy(gameObject);
        }

    
  }
}
