using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] int playerScore = 0;
    [SerializeField] int oneUp = 1000;
    [SerializeField] int toNextUp = 0;


    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);//if we have more than one, destroy the newly created one and keep the old one
        }
        else
        {
            DontDestroyOnLoad(gameObject);//if there is no existing game(Object) dont destroy it
        }
    }

    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = playerScore.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
            livesText.text = playerLives.ToString();
        }
        else
        {
            ResetGameSession();
        }
    }

    void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ScenePersistReset();
        SceneManager.LoadScene(0);//load first level if run out of lives
        Destroy(gameObject);//destroy this game session so they can have a fresh one
    }

    void TakeLife()
    {
        playerLives -= 1;
        StartCoroutine(Reset());
        
    }
    IEnumerator Reset()
    {
        yield return new WaitForSecondsRealtime(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ScoreIncrement(int points)
    {
        playerScore += points;
        //Debug.Log("incremented points by " + points);
        toNextUp += points;
        scoreText.text = playerScore.ToString();
        //Debug.Log(toNextUp);
        if (toNextUp >= oneUp)
        {
            Debug.Log("One up achieved");
            playerLives += 1;
            livesText.text = playerLives.ToString();
            toNextUp -= oneUp;
        }
    }
}
