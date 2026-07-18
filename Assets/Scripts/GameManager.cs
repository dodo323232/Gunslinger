using UnityEngine;
using TMPro;
using System.Collections;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private AiPlayer aiPlayer;
    [SerializeField]
    public Animator[] frame1Animator;
    [SerializeField]
    public GameObject DifPanel;
    public static GameManager instance;

    public bool gameStart = false;
    public bool readyStart = false;
    public float randomTime;
    public enum Difficulty {Hard, Normal, Easy}; // 오늘 배운 enum, Hard는 0 Normal은 1 Easy는 2
    // 여기서 Difficulty는 타입을 뜻한다
    public Difficulty d = Difficulty.Easy;
    public float[] minReaction = {150f,230f,260f}; // ai 최소 반응속도
    public float[] maxReaction = {200f,255f,300f}; // ai 최대 반응속도
    private DifficultyButtonGroup dif;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    AudioClip shoot;
    [SerializeField]
    AudioClip gameOverSound;
    [SerializeField]
    AudioClip trackSound;
    [SerializeField]
    private GameObject ballPrefab;
    private AudioSource audioSource;
    [SerializeField]
    private GameObject menuPanel;
    private Coroutine startTimerCoroutine;
    Vector3 ballLocate = new Vector3(10.62f,-3.29f,0f);
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        d = Difficulty.Easy;
    }

    void Start()
    {
        // StartCoroutine(StartTimer());
        audioSource = GetComponent<AudioSource>();
        dif = FindAnyObjectByType<DifficultyButtonGroup>(FindObjectsInactive.Include);
        Audio.instance.MenuAudio();
    }


    public IEnumerator StartTimer()    // 게임 시작시 타이머 랜덤
    {
        frame1Animator[(int)d].SetTrigger("IdleTrigger");
        yield return new WaitForSeconds(1.5f);            
        gameStart = false;
        readyStart = false;
        text.SetText("READY");
        // Audio.instance.Heartbeat();
        readyStart = true;
        randomTime = Random.Range(2.5f,5f);
        Instantiate(ballPrefab,ballLocate,Quaternion.identity);
        yield return new WaitForSeconds(randomTime);
        text.SetText("shoot!");
        // Audio.instance.Heartbeat();
        TimeManager.instance.TimeStart();
        aiPlayer.DecideReactionTime();
        gameStart = true;
    }
    
    public void Shoot()
    {                    
        frame1Animator[(int)d].ResetTrigger("IdleTrigger"); // idletrigger을 너무 빨리 실행 했으므로 true가 되었다 그래서 shoottrigger을 했지만
        frame1Animator[(int)d].SetTrigger("ShootTrigger");  // exittime이 끝나고 바로 idle로 돌아가게 되는 버그 발생.
    }                                                       // 그래서 resettrigger을 함으로서 다시 false로 만듦

    public void TimeRestart()
    {
        if (startTimerCoroutine != null)
        {
            StopCoroutine(startTimerCoroutine);
        }
        gameStart = false; 
        readyStart = false;
        text.SetText("");
        startTimerCoroutine = StartCoroutine(StartTimer());
    }

    public void ShootSound()
    {
        audioSource.PlayOneShot(shoot);
    }
    public void GameOverSound()
    {
        audioSource.PlayOneShot(gameOverSound);
    }
    public void TrackSound()
    {
        audioSource.PlayOneShot(trackSound);
    }
    public void PlayStart()
    {
        // menuPanel.SetActive(false);
        // startTimerCoroutine = StartCoroutine(StartTimer());
        dif.selectAiPlayer();
        ScoreUi.instance.PlayAgain();
        Audio.instance.MenuAudio();
    }
    public void DifButton()
    {
        DifPanel.SetActive(true);    
    }
    public void ExitButton()
    {
        DifPanel.SetActive(false);
    }
}
