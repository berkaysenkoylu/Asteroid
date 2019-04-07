using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public Text scoreText;
    public Text waveNumberText;
    public GameObject gameOverText;
    public GameObject restartText;
    public GameObject canvas;
    public float score;

    bool isPlayerAlive;
    Animator animCanvas;
    Fading fading;

    void Start ()
    {
        score = 0;
        isPlayerAlive = true;
        scoreText.text = "Score: " + score;

        animCanvas = canvas.GetComponent<Animator>();
    }
	
	void Update ()
    {
        UpdateScore();

        if (isPlayerAlive == false)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(1);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
	}

    public void OnPlayerDeath()
    {
        Debug.Log("Player is death, R.I.P!");

        // Change the player's state to death
        isPlayerAlive = false;

        // Play fade animation
        animCanvas.SetBool("isPlayerDead", true);
    }

    public void IncreaseScore(float scoreIncrement)
    {
        score += scoreIncrement;
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateWaveNumber(int waveNumber)
    {
        waveNumberText.text = "Wave: " + waveNumber;
    }
}
