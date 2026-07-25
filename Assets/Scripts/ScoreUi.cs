using TMPro;
using Unity.Collections;
using UnityEngine;
public class ScoreUi : MonoBehaviour
{
    [SerializeField]
    private Player player;
    public static ScoreUi instance;
    [SerializeField]
    private GameObject Menu;

    [SerializeField]
    private GameObject playerWinPanel;
    [SerializeField]
    private GameObject aiWinPanel;

    [SerializeField]
    private TextMeshProUGUI aiAverage;
    [SerializeField]
    private TextMeshProUGUI playerAverage;
    private const int WinsNeeded = 1;
    public int playerScore = 0;
    public int aiScore = 0;
    [SerializeField]
    public TextMeshProUGUI playerScoText;
    [SerializeField]
    public TextMeshProUGUI aiScoText;
    [SerializeField]
    private GameObject highScorePanel;
    [SerializeField]
    private TextMeshProUGUI highReactionText;
    private float bestReaction = -1f;

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
        playerScoText.SetText(playerScore.ToString());
        if (playerScore >= WinsNeeded)
        {
            Invoke("PlayerWin", 2.5f);
            GameManager.instance.AiDie(true);
            GameManager.instance.shootImg.SetActive(false);
            GameManager.instance.readyImg.SetActive(false);
        }
        else
        {
            Invoke("NextRound", 2.5f);
        }
    }

    public void AiScoreUp()
    {
        aiScore++;
        aiScoText.SetText(aiScore.ToString());
        if (aiScore >= WinsNeeded)
        {
            Invoke("AiWin", 2.5f);
            GameManager.instance.shootImg.SetActive(false);
            GameManager.instance.readyImg.SetActive(false);
        }
        else
        {
            Invoke("NextRound", 2.5f);
        }
    }

    private void PlayerWin()
    {
        playerWinPanel.SetActive(true);
        Audio.instance.StopGameMusic();
        Audio1.instance.GameWinSound();
        ScoreUi.instance.AverageText();
        playerScore = 0;
        aiScore = 0;
        playerScoText.SetText(playerScore.ToString());
        aiScoText.SetText(aiScore.ToString());
        GameManager.instance.scorePanel.SetActive(false);
        GameManager.instance.reactionTextPanel.SetActive(false);
    }

    private void AiWin()
    {
        aiWinPanel.SetActive(true);
        Audio.instance.StopGameMusic();
        GameManager.instance.GameOverSound();
        ScoreUi.instance.AverageText();
        playerScore = 0;
        aiScore = 0;
        playerScoText.SetText(playerScore.ToString());
        aiScoText.SetText(aiScore.ToString());
        GameManager.instance.scorePanel.SetActive(false);
        GameManager.instance.reactionTextPanel.SetActive(false);
    }

    private void NextRound() // 아직 3승을 못했으면 다음 라운드로 진행
    {
        GameManager.instance.shootImg.SetActive(false);
        GameManager.instance.TrackSound();
        player.Stop();
        TimeManager.instance.TimeReset();
        GameManager.instance.TimeRestart();
        player.ReStart();
        GameManager.instance.PlayerDie(false);
    }

    public void PlayAgain()
    {
        GameManager.instance.TrackSound();
        Audio.instance.PlayGameMusic();
        playerWinPanel.SetActive(false);
        aiWinPanel.SetActive(false);
        Menu.SetActive(false);
        playerScore = 0;
        aiScore = 0;
        player.Stop();
        TimeManager.instance.TimeReset();
        GameManager.instance.TimeRestart();
        player.ReStart();
        TimeManager.instance.averageReaction = 0;
        TimeManager.instance.averageReaction1 = 0;
        GameManager.instance.scorePanel.SetActive(true);
        GameManager.instance.mistakePanel.SetActive(false);   
        TimeManager.instance.reactionText.SetText("");
        GameManager.instance.reactionTextPanel.SetActive(true);
        GameManager.instance.PlayerDie(false);
    }
    public void AverageText()
    {
        TimeManager.instance.averageReaction = TimeManager.instance.averageReaction / TimeManager.instance.averageReaction1;
        aiAverage.SetText(TimeManager.instance.averageReaction.ToString("F1") + "ms"); // ToString("F1")은 소수 첫째자리까지
        playerAverage.SetText(TimeManager.instance.averageReaction.ToString("F1") + "ms");
    }
    public void MenuButton()
    {
        GameManager.instance.TrackSound();
        GameManager.instance.frame1Animator[(int)GameManager.instance.d].SetTrigger("IdleTrigger");
        Audio.instance.StopGameMusic();
        Audio.instance.MenuAudio();
        playerWinPanel.SetActive(false);
        aiWinPanel.SetActive(false);
        Menu.SetActive(true);
        GameManager.instance.mistakePanel.SetActive(false);
        GameManager.instance.scorePanel.SetActive(false);
    }
    public void MistakePanel()
    {

    }
    public void HighScoreButton()
    {
        GameManager.instance.TrackSound();
        highScorePanel.SetActive(true);
        UpdateHighReactionText();
    }
    public void CloseHighScore()
    {
        GameManager.instance.TrackSound();
        highScorePanel.SetActive(false);
    }
    private void UpdateHighReactionText() // 이번 판 평균 반응속도와 지금까지의 최고기록 중 더 빠른(작은) 값을 기록으로 채택
    {
        float current = TimeManager.instance.averageReaction;
        if (current > 0 && (bestReaction < 0 || current < bestReaction))
        {
            bestReaction = current;
        }
        highReactionText.SetText(bestReaction < 0 ? "--" : bestReaction.ToString("F1") + "ms");
    }
}
