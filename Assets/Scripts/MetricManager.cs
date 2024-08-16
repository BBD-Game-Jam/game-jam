using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MetricManager : MonoBehaviour
{
    public GameObject character;
    public TextMeshProUGUI score;
    public TextMeshProUGUI velocity;
    public TextMeshProUGUI velocityX;
    public TextMeshProUGUI velocityY;
    public TextMeshProUGUI gravity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void updateScore()
    {
        score.text = $"Score: {character.transform.position.x}";
    }

    void updateVelocity()
    {
        Rigidbody2D rg = character.GetComponent<Rigidbody2D>();
        velocity.text = $"Velocity: {rg.velocity.magnitude}";
        velocityX.text = $"Vx: {rg.velocity.x}";
        velocityY.text = $"Vy: {rg.velocity.y}";
        gravity.text = $"Gravity: {rg.gravityScale*9.8} m/s^2";
    }

    // Update is called once per frame
    void Update()
    {
        updateScore();
        updateVelocity();
    }
}
