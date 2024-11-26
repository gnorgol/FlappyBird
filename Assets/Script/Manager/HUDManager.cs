using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SocialPlatforms;

public class HUDManager : MonoBehaviour
{
    public GameObject GameOverPanel;
    public GameObject ScorePanel;
    public TextMeshProUGUI ScoreText;


    private void OnEnable()
    {
        EventManager.OnGameOver += GameOver;
        EventManager.OnScore += UpdateScoreDisplay;
    }
    private void OnDisable()
    {
        EventManager.OnGameOver -= GameOver;
        EventManager.OnScore -= UpdateScoreDisplay;
    }
    private void GameOver()
    {
        Debug.Log("Game Over");
        GameOverPanel.SetActive(true);
    }
    private void UpdateScoreDisplay(int newScore)
    {
        Debug.Log("Gain Point");
        ScoreText.text = "Score: " + newScore;

    }
    private void Start()
    {
        GameOverPanel.SetActive(false);
    }

}
