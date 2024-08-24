using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 


public class GameOverClick : MonoBehaviour
{
    public void OnButtonClick()
    {
        Debug.Log("Button clicked!");
        ResetScene();
    }

    public void ResetScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
