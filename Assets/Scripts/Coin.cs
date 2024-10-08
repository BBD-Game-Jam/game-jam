using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
  // Start is called before the first frame update
  public LogicScript logic;
  public Animation ani;
  public AudioClip coinCollected;
    private RaycastHit2D hit;

    void Start()
  {
    logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    var audioSource = GameObject.FindGameObjectWithTag("CoinCollectedAudio").GetComponent<AudioSource>();
    coinCollected = audioSource.clip;
  }

  // Update is called once per frame
  void Update()
  {
        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;
        hit = Physics2D.Raycast(new Vector2(x, y), Vector2.down);
        if (hit.collider != null)
        {
            Debug.Log(y);
            y = hit.point.y;
            gameObject.transform.position = new Vector2(x, y);
        }
    }

  private void OnTriggerEnter2D(Collider2D collision)
  {

    /*if (collision.gameObject.layer == 3)
    {*/
    logic.addPoints();
    /*}*/
    Debug.Log("Collected coin");
    AudioSource.PlayClipAtPoint(coinCollected, transform.position);
    Destroy(gameObject);

  }
}
