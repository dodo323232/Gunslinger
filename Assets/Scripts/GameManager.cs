using UnityEngine;
using TMPro;
using System.Collections;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private AiPlayer aiPlayer;
    [SerializeField]
    private Animator frame1Animator;
    [SerializeField]
    public GameObject DifPanel;
    public static GameManager instance;

    public bool gameStart = false;
    public bool readyStart = false;
    public float randomTime;
    
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

    Vector3 ballLocate = new Vector3(10.62f,-3.29f,0f);
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        // StartCoroutine(StartTimer());
        audioSource = GetComponent<AudioSource>();
    }


    IEnumerator StartTimer()    // 게임 시작시 타이머 랜덤
    {
        yield return new WaitForSeconds(1.5f);
        gameStart = false;
        readyStart = false;
        text.SetText("READY");
        readyStart = true;
        randomTime = Random.Range(2f,5f);
        Instantiate(ballPrefab,ballLocate,Quaternion.identity);
        yield return new WaitForSeconds(randomTime);
        text.SetText("shoot!");
        TimeManager.instance.TimeStart();
        aiPlayer.DecideReactionTime();
        gameStart = true;
    }
    
    public void Shoot()
    {
        frame1Animator.SetTrigger("ShootTrigger");
    }

    public void TimeRestart()
    {
        gameStart = false; 
        readyStart = false;
        text.SetText("");
        StartCoroutine(StartTimer());
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
        menuPanel.SetActive(false);
        StartCoroutine(StartTimer());
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
