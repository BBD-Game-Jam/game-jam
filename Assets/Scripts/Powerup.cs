using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    // Start is called before the first frame update
    public LogicScript logic;

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int type = (int)Mathf.Round(Random.Range(0, 2));
        if (collision.gameObject.layer == 3)
        {
            logic.Powerup(type);
        }

        Destroy(gameObject);
    }
}
