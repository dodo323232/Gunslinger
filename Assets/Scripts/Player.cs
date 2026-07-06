using System.Collections;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private Coroutine reactionCoroutine;

    void Start()
    {
        animator = GetComponent<Animator>();
        reactionCoroutine = StartCoroutine(PlayerReaction());
    }

    IEnumerator PlayerReaction()
    {
        yield return new WaitUntil(() => GameManager.instance.readyStart);
        
        while (true)
        {
            TimeManager.instance.TimeRecord();
            if(TimeManager.instance.recordTime > TimeManager.instance.randomReaction)
            {
                GameManager.instance.ShootSound();
                GameManager.instance.Shoot();
                Debug.Log("너무 오소이~ ai 승 : "+TimeManager.instance.randomReaction);
                TimeManager.instance.TimeStop();
                ScoreUi.instance.AiScoreUp();
                break;
            }

            if (GameManager.instance.readyStart && Input.GetMouseButtonDown(0))
            {
                if (!GameManager.instance.gameStart)
                {
                    Mistake();
                    GameManager.instance.ShootSound();
                    yield return new WaitForSeconds(2f);
                }
                else
                {
                    Success();
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
