using UnityEngine;
public class ScoreUi : MonoBehaviour
{
    [SerializeField]
    private Player player;
    public static ScoreUi instance;

    [SerializeField]
    private GameObject playerWinPanel;
    [SerializeField]
    private GameObject aiWinPanel;

    private const int WinsNeeded = 5;
    public int playerScore = 0;
    public int aiScore = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlayerScoreUp()
    {
        playerScore++;
        if (playerScore >= WinsNeeded)
        {
            Invoke("PlayerWin", 2.5f);
        }
        else
        {
            Invoke("NextRound", 2.5f);
        }
    }

    public void AiScoreUp()
    {
        aiScore++;
        if (aiScore >= WinsNeeded)
        {
            Invoke("AiWin", 2.5f);
        }
        else
        {
            Invoke("NextRound", 2.5f);
        }
    }

    private void PlayerWin()
    {
        playerWinPanel.SetActive(true);
        GameManager.instance.GameOverSound();
    }

    private void AiWin()
    {
        aiWinPanel.SetActive(true);
        GameManager.instance.GameOverSound();
    }

    private void NextRound() // 아직 3승을 못했으면 다음 라운드로 진행
    {
        GameManager.instance.TrackSound();
        player.Stop();
        TimeManager.instance.TimeReset();
        GameManager.instance.TimeRestart();
        player.ReStart();
    }

    public void PlayAgain()
    {
        GameManager.instance.TrackSound();
        playerWinPanel.SetActive(false);
        aiWinPanel.SetActive(false);
        playerScore = 0;
        aiScore = 0;
        player.Stop();
        TimeManager.instance.TimeReset();
        GameManager.instance.TimeRestart();
        player.ReStart();
    }
}
