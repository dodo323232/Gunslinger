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
    public Animator playerAnimator;
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
    AudioClip gameWinSound;
    [SerializeField]
    AudioClip trackSound;
    [SerializeField]
    private GameObject ballPrefab;
    private AudioSource audioSource;
    [SerializeField]
    private GameObject menuPanel;
    private Coroutine startTimerCoroutine;
    [SerializeField]
    public GameObject scorePanel;
    [SerializeField]
    public GameObject mistakePanel;
    [SerializeField]
    public GameObject readyImg;
    [SerializeField]
    public GameObject shootImg;
    [SerializeField]
    public GameObject reactionTextPanel;
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
        readyImg.SetActive(true);
        // Audio.instance.Heartbeat();
        readyStart = true;
        randomTime = Random.Range(3.5f,8f);
        Instantiate(ballPrefab,ballLocate,Quaternion.identity);
        yield return new WaitForSeconds(randomTime);
        readyImg.SetActive(false);
        shootImg.SetActive(true);
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

    public void AiDie(bool die)
    {
        if (die)
        {
            frame1Animator[(int)d].ResetTrigger("IdleTrigger");
            frame1Animator[(int)d].SetTrigger("DieTrigger");
            StartCoroutine(WinImpact());
        }
        else
        {
            frame1Animator[(int)d].ResetTrigger("DieTrigger");
            frame1Animator[(int)d].SetTrigger("IdleTrigger");
        }
    }

    private IEnumerator WinImpact() // 승리 임팩트 : 히트스톱(슬로우모션) + 카메라 흔들림
    {
        Time.timeScale = 0.05f; // 완전히 0으로 멈추면 Invoke("PlayerWin", ...) 타이머도 같이 멈추므로 살짝만 느리게
        yield return new WaitForSecondsRealtime(0.08f);
        Time.timeScale = 1f;

        Camera cam = Camera.main;
        if (cam == null) yield break;

        Vector3 originalPos = cam.transform.localPosition; // 원래 카메라 위치 기억
        float shakeDuration = 0.25f;
        float magnitude = 0.15f;
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float damper = 1f - (elapsed / shakeDuration); // 시간이 지날수록 흔들림이 잦아듦
            Vector2 offset = Random.insideUnitCircle * magnitude * damper; // Random.insideUnitCircle 원 안에 아무 점이나 찍는거
            cam.transform.localPosition = originalPos + new Vector3(offset.x, offset.y, 0f);
            elapsed += Time.unscaledDeltaTime; // 매 프레임마다 실제로 걸린 시간을 더했다
            yield return null;
        }
        cam.transform.localPosition = originalPos; // 원래 카메라 위치로 되돌리기
    }
    public void PlayerDie(bool die)
    {
        if (die)
        {
            playerAnimator.ResetTrigger("IdleTrigger");
            playerAnimator.SetTrigger("DieTrigger");
        }
        else
        {
            playerAnimator.ResetTrigger("DieTrigger");
            playerAnimator.SetTrigger("IdleTrigger");
        }
    }

    public void TimeRestart()
    {
        StopTimer();
        gameStart = false;
        readyStart = false;
        startTimerCoroutine = StartCoroutine(StartTimer());
    }

    public void StopTimer()
    {
        if (startTimerCoroutine != null)
        {
            StopCoroutine(startTimerCoroutine);
        }
    }

    public void ShootSound()
    {
        audioSource.PlayOneShot(shoot);
    }
    public void GameOverSound()
    {
        audioSource.PlayOneShot(gameOverSound);
    }
    public void GameWinSound()
    {
        audioSource.PlayOneShot(gameWinSound);
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
        reactionTextPanel.SetActive(true);
        Invoke("Wait",0.5f); // 0.5초뒤 게임 음악 재생
    }
    private void Wait()
    {
        Audio.instance.PlayGameMusic();
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
