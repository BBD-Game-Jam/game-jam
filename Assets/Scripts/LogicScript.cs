using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogicScript : MonoBehaviour
{
  // Start is called before the first frame update
  public MetricManager metrics;
  public PinguScript character;
  public TextMeshProUGUI powerUpMessage;
  public float powerUpTime = 10f;
  // Start is called before the first frame update

  private bool hasPower = false;
  private float countDownTime = 10f;
  private string power;
  public float powerupVelocity = 50f;
  void Start()
  {

  }

  void Update()
  {
    if (hasPower)
    {
      UpdatePowerMessage();
    }
  }

  public void addPoints()
  {
    metrics.addPoints();
  }

  public void Powerup(int type)
  {
    RemovePower("something");
    countDownTime = powerUpTime;
    hasPower = true;
    if (type == 0)
    {
      power = "Flow state (2x points)";
      metrics.SetPointMultiplier(2);
    }
    else if (type == 1)
    {
      power = "Flow state (speed)";
      character.SetVelocity(powerupVelocity);
    }
  }

  // Update is called once per frame
  private void UpdatePowerMessage()
  {
    if (countDownTime >= 0)
    {
      countDownTime = countDownTime - Time.deltaTime;
      powerUpMessage.text = $"{power}: {(int)countDownTime}s";
    }
    else
    {
      hasPower = false;
      powerUpMessage.text = "";
      RemovePower(power);
    }
  }

  private void RemovePower(string power)
  {
    character.SetVelocity(10f);
    metrics.SetPointMultiplier(1);
    hasPower = false;
  }

}
