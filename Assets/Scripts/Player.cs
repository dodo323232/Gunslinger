using System.Collections;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private Coroutine reactionCoroutine;
    private bool aiWin = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        reactionCoroutine = StartCoroutine(PlayerReaction());
    }

    IEnumerator PlayerReaction()
    {
        yield return new WaitUntil(() => GameManager.instance.readyStart);
        aiWin = false;

        while (true)
        {
            TimeManager.instance.TimeRecord();
            if(!aiWin && TimeManager.instance.recordTime > TimeManager.instance.randomReaction)
            {
                aiWin = true;
                GameManager.instance.ShootSound();
                GameManager.instance.Shoot();
                GameManager.instance.WinImpactEffect();
                GameManager.instance.PlayerDie(true);
                Debug.Log("너무 오소이~ ai 승 : "+TimeManager.instance.randomReaction);
            }

            if (GameManager.instance.readyStart && Input.GetMouseButtonDown(0))
            {
                if (!GameManager.instance.gameStart)
                {
                    Mistake();
                    // GameManager.instance.ShootSound();
                    // GameManager.instance.Shoot();
                    // ScoreUi.instance.AiScoreUp();
                    break;
                }
                else if (aiWin)
                {
                    TimeManager.instance.TimeStop();
                    Debug.Log("이미 패배, 반응속도만 기록 : "+TimeManager.instance.reaction);
                    ScoreUi.instance.AiScoreUp();
                    GameManager.instance.readyStart = false;
                    TimeManager.instance.averageReaction1 += 1;
                    break;
                }
                else
                {
                    Success();
                    GameManager.instance.WinImpactEffect();
                    GameManager.instance.ShootSound();
                    Debug.Log(TimeManager.instance.randomReaction);
                    TimeManager.instance.WinLoss();
                    break;
                }
            }

            yield return null;
        }
    }

    private void Mistake()
    {
        GameManager.instance.StopTimer();
        // ScoreUi.instance.PlayAgain();
        ScoreUi.instance.playerScore = 0;
        ScoreUi.instance.aiScore = 0;
        ScoreUi.instance.playerScoText.SetText(ScoreUi.instance.playerScore.ToString());
        ScoreUi.instance.aiScoText.SetText(ScoreUi.instance.aiScore.ToString());
        GameManager.instance.mistakePanel.SetActive(true);
        GameManager.instance.shootImg.SetActive(false);
        GameManager.instance.readyImg.SetActive(false);
        // Audio.instance.GameAudio();
        GameManager.instance.GameOverSound();
        GameManager.instance.reactionTextPanel.SetActive(false);
        Debug.Log("실패");
    }

    private void Success()
    {
        TimeManager.instance.TimeStop();
        animator.SetTrigger("PlayerShoot");
        Debug.Log("성공"+TimeManager.instance.reaction);
        GameManager.instance.readyStart = false;
    }

    public void ReStart()
    {
        reactionCoroutine = StartCoroutine(PlayerReaction());
    }

    public void Stop()
    {
        if (reactionCoroutine != null)
        {
            StopCoroutine(reactionCoroutine);
        }
    }

}
