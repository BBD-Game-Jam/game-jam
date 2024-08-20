using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicScript : MonoBehaviour
{
    // Start is called before the first frame update
    public MetricManager metrics; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addPoints()
    {
        metrics.addPoints();
    }
}
