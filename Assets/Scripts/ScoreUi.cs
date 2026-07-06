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

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlayerScoreUp()
    {
        Invoke("PlayerWin", 2.5f);
    }

    public void AiScoreUp()
    {
        Invoke("AiWin", 2.5f);
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

    public void PlayAgain()
    {
        GameManager.instance.TrackSound();
        playerWinPanel.SetActive(false);
        aiWinPanel.SetActive(false);
        player.Stop();
        TimeManager.instance.TimeReset();
        GameManager.instance.TimeRestart();
        player.ReStart();
    }
}
