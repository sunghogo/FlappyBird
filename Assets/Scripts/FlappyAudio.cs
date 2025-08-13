
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
    
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClip(FlappyClip clip)
    {
        switch (clip)
        {
            case FlappyClip.Wing:
                audioSource.clip = wingClip;
                break;
            case FlappyClip.Swoosh:
                audioSource.clip = swooshClip;
                break;
            case FlappyClip.Point:
                audioSource.clip = pointClip;
                break;
            case FlappyClip.Hit:
                audioSource.clip = hitClip;
                break;
            case FlappyClip.Die:
                audioSource.clip = dieClip;
                break;
            default:
                return;
        }
        audioSource.Play();
    }
}
