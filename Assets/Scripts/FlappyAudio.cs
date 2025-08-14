
using UnityEngine;

public enum FlappyClip
{
    Wing,
    Swoosh,
    Point,
    Hit,
    Die,
}

public class FlappyAudio : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] AudioClip wingClip;
    [SerializeField] AudioClip swooshClip;
    [SerializeField] AudioClip pointClip;
    [SerializeField] AudioClip hitClip;
    [SerializeField] AudioClip dieClip;
    [SerializeField] AudioClip bgmClip;

    
    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        GameManager.OnGameStart += HandleGameStart;
        GameManager.OnGameOver += HandleGameOver;
    }

    void OnDestroy()
    {
        GameManager.OnGameStart -= HandleGameStart;
        GameManager.OnGameOver -= HandleGameOver;
    }

    void Start()
    {
        audioSource.clip = bgmClip;
        audioSource.loop = true;
    }

    void HandleGameStart() {
        if (audioSource.gameObject.activeInHierarchy) {
            audioSource.Play();
        }
    }

    void HandleGameOver() {
        audioSource.Stop();
    }

    public void PlayClip(FlappyClip clip)
    {
        AudioClip playedClip;
        switch (clip)
        {
            case FlappyClip.Wing:
                playedClip = wingClip;
                break;
            case FlappyClip.Swoosh:
                playedClip = swooshClip;
                break;
            case FlappyClip.Point:
                playedClip = pointClip;
                break;
            case FlappyClip.Hit:
                playedClip = hitClip;
                break;
            case FlappyClip.Die:
                playedClip = dieClip;
                break;
            default:
                return;
        }
        audioSource.PlayOneShot(playedClip);
    }
}
