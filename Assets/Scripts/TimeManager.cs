using UnityEngine;
using System.Diagnostics;
using Unity.Mathematics;
using System;
using TMPro;
public class TimeManager : MonoBehaviour
{
    Stopwatch stopwatch = new Stopwatch();
    [SerializeField]
    private Player player;
    public static TimeManager instance;
    
    public float recordTime;
    public float reaction;
    public float averageReaction = 0f;
    public int averageReaction1 = 0;
    public float randomReaction;
    [SerializeField]
    public TextMeshProUGUI reactionText;
    void Awake()
    {
        if( instance == null)
        {
            instance = this;
        }
    }

    public void TimeStart()
    {
        stopwatch.Start();
        reactionText.SetText("");
    }

    public void TimeStop()
    {
        stopwatch.Stop();
        double rawMs = stopwatch.Elapsed.TotalMilliseconds; 
        reaction = (float)(System.Math.Round(rawMs,1)); // 더블을 소수 한자리
        averageReaction += reaction;
        reactionText.SetText(reaction.ToString());
    }

    public void TimeRecord()
    {
        double rawMs = stopwatch.Elapsed.TotalMilliseconds;
        recordTime = (float)(System.Math.Round(rawMs,1));
    }
    public void WinLoss()
    {
        if (reaction < randomReaction)
        {
            UnityEngine.Debug.Log("Player 승");
            ScoreUi.instance.PlayerScoreUp();
            averageReaction1 += 1;
            GameManager.instance.readyStart = false;
        }
        else if (reaction > randomReaction)
        {
            UnityEngine.Debug.Log("Ai 승");
            GameManager.instance.Shoot();
            ScoreUi.instance.AiScoreUp();
            averageReaction1 += 1;
            GameManager.instance.readyStart = false;
        }

        else if(randomReaction == reaction)
        {
            UnityEngine.Debug.Log("무승부");
            averageReaction1 += 1;
            player.Stop();
            GameManager.instance.TimeRestart();
            player.ReStart();
            GameManager.instance.readyStart = false;
        }
        
    }
    
    public void TimeReset()
    {
        stopwatch.Reset();
        recordTime = 0;
    }
}
