using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MetricManager : MonoBehaviour
{
    public GameObject character;
    public TextMeshProUGUI score;
    public TextMeshProUGUI velocity;
    public TextMeshProUGUI velocityX;
    public TextMeshProUGUI velocityY;
    public TextMeshProUGUI gravity;
    public TextMeshProUGUI time;
    public TextMeshProUGUI velocityAng;
    float playTime = 0;
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
        velocity.text = $"Velocity: {rg.velocity.magnitude} m/s";
        velocityX.text = $"Vx: {rg.velocity.x} m/s";
        velocityY.text = $"Vy: {rg.velocity.y} m/s";
        velocityAng.text = $"Vang: {rg.angularVelocity} m/s";
        gravity.text = $"Gravity: {rg.gravityScale*9.8} m/s^2";
    }

    void UpdateTime()
    {
        playTime = playTime + Time.deltaTime;
        time.text = $"Time: {playTime} s";
    }

    // Update is called once per frame
    void Update()
    {
        updateScore();
        updateVelocity();
        UpdateTime();
    }
}
