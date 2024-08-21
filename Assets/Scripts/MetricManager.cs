using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MetricManager : MonoBehaviour
{
    public GameObject character;
    public GameObject enemy;
        public Camera cam;

    public TextMeshProUGUI score;
    public TextMeshProUGUI distance;
    public TextMeshProUGUI enemyDistance;
    public TextMeshProUGUI velocity;
    public TextMeshProUGUI velocityX;
    public TextMeshProUGUI velocityY;
    public TextMeshProUGUI gravity;
    public TextMeshProUGUI time;
    public TextMeshProUGUI velocityAng;
    private float playTime = 0;
    private float savedDistance = 0;
    private float distanceTraveled = 0;
    private float points = 0;
    private float finalScore = 0;
    private float oldX = 0;
    private float enemyDistanceVar = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void updatePlayerDistance()
    {
        if (character.transform.position.x < oldX)
        {
            savedDistance = distanceTraveled;
            
        }
        distanceTraveled = (int)(savedDistance + character.transform.position.x);
        distance.text = $"Distance: {distanceTraveled} m";
        oldX = character.transform.position.x;

    }

    private void updateScore()
    {
        finalScore = (int)(distanceTraveled / 50f) + points;
        score.text = $"Score: {finalScore}";
    }

    void updateEnemyDistance()
    {
        if (cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane)).x < enemy.transform.position.x)
        {
            enemyDistance.enabled = false;
        }
        else{
                        enemyDistance.enabled = true;

        }
        float distance = Vector3.Distance (character.transform.position, enemy.transform.position);
        enemyDistance.text = $"{(int)distance}m";
    }

    [ContextMenu("Increase Score")]
    public void addPoints()
    {
        points = points + 10;
    }

    private void updateVelocity()
    {
        Rigidbody2D rg = character.GetComponent<Rigidbody2D>();
        velocity.text = $"Velocity: {rg.velocity.magnitude} m/s";
        velocityX.text = $"Vx: {rg.velocity.x} m/s";
        velocityY.text = $"Vy: {rg.velocity.y} m/s";
        velocityAng.text = $"Vang: {rg.angularVelocity} m/s";
        gravity.text = $"Gravity: {rg.gravityScale * 9.8} m/s^2";
    }

    private void UpdateTime()
    {
        playTime = playTime + Time.deltaTime;
        time.text = $"Time: {playTime} s";
    }

    // Update is called once per frame
    void Update()
    {
        updateEnemyDistance();
        updatePlayerDistance();
        updateScore();
        updateVelocity();
        UpdateTime();
    }
}
