using UnityEngine;

public class Audio1 : MonoBehaviour
{
    public static Audio1 instance;
    [SerializeField]
    private AudioClip gameWinSound;
    private AudioSource audioSource;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        audioSource = GetComponent<AudioSource>();
    }
    public void GameWinSound()
    {
        audioSource.PlayOneShot(gameWinSound);
    }
}
