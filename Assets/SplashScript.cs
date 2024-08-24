using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SplashScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveUp = 5f;
    void Start()
    {
        // Play animation
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * moveUp * Time.deltaTime;
        
    }
}
