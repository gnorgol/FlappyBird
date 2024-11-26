using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public delegate void GameDelegate();
    public static event GameDelegate OnGameOver;
    public static event GameDelegate OnGainPoint;
    //Event change score
    public delegate void ScoreDelegate(int score);
    public static event ScoreDelegate OnScore;

    private void Awake()
    {
        instance = this;
    }
    public void TriggerGameOver()
    {
        if (OnGameOver != null)
        {
            OnGameOver();
        }
    }
    public void TriggerGainPoint()
    {
        if (OnGainPoint != null)
        {
            OnGainPoint?.Invoke();
        }
    }
    public void TriggerScore(int score)
    {
        if (OnScore != null)
        {
            OnScore(score);
        }
    }
}
