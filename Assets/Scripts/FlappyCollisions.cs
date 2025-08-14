using UnityEngine;

public class FlappyCollisions : MonoBehaviour
{
    [SerializeField] Vector3 startingPosition;
    FlappyAudio flappyAudio;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GameManager.Instance.GameOver && (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle")))
        {
            GameManager.Instance.EndGame();
            flappyAudio.PlayClip(FlappyClip.Hit);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.Instance.GameOver && collision.CompareTag("Score"))
        {
            GameManager.Instance.IncrementScore();
            flappyAudio.PlayClip(FlappyClip.Point);
        }
    }

    void Awake()
    {
        GameManager.OnGameStart += HandleGameStart;
        GameManager.OnGameOver += HandleGameOver;
    }

    void OnDestroy()
    {
        GameManager.OnGameStart -= HandleGameStart;
        GameManager.OnGameOver -= HandleGameOver;
    }

    void HandleGameOver()
    {
        CoroutineRunner.StartFadeOutThenDisable(gameObject);
        flappyAudio.PlayClip(FlappyClip.Die);
    }

    void HandleGameStart()
    {
        transform.position = startingPosition;
        CoroutineRunner.StartEnableThenFadeIn(gameObject, 0.25f, 0.75f);
    }

    void Start()
    {
        flappyAudio = GetComponent<FlappyAudio>();
        startingPosition = transform.position;
        gameObject.SetActive(false);
    }
}
