using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField] private AudioSource heartbeatSource;
    [SerializeField] AudioClip heartbeatSound;
    [SerializeField] private AudioSource menuSource;
    [SerializeField] AudioClip menuSound;
    [SerializeField] private AudioSource gameMusicSource;
    [SerializeField] AudioClip gameMusic;
    private AudioSource audioSource;

    public static Audio instance;
    private bool HeartbeatOn = false;
    private bool menuOn = true;
    private bool gameStart = true;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void Heartbeat()
    {
        if (!HeartbeatOn)
        {
            heartbeatSource.clip = heartbeatSound;
            heartbeatSource.loop = true; 
            heartbeatSource.Play();
            HeartbeatOn = true;
        }
        else
        {
            heartbeatSource.Stop();
            HeartbeatOn = false;
        }
    }

    public void MenuAudio()
    {
        if (menuOn)
        {
            menuSource.clip = menuSound;
            menuSource.loop = true;
            menuSource.Play();
            menuOn = false;
        }
        else
        {
            menuSource.Stop();
            menuOn = true;
        }
    }
    public void GameAudio()
    {
        if (gameStart)
        {
            Debug.Log("음악 시작!");
            gameMusicSource.clip = gameMusic;
            gameMusicSource.loop = true;
            gameMusicSource.Play();
            gameStart = false;
        }
        else
        {
            Debug.Log("음악 !");
            gameMusicSource.Stop();
            gameStart = true;
        }
    }
}
