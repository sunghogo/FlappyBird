using UnityEngine;

public class StartMessageSprite : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] float floatingPeriod = 1f;
    [SerializeField] float floatingAmplitude = 0.05f;

    bool wasClicked = false;
    float t = 0f;
    Vector3 startingPosition;
    
    void Awake()
    {
        GameManager.OnGameStart += HandleGameStart;
        GameManager.OnScreenStart += HandleScreenStart;
    }

    void Start() {
        startingPosition = transform.position;
    }

    void OnDestroy()
    {
        GameManager.OnGameStart -= HandleGameStart;
        GameManager.OnScreenStart -= HandleScreenStart;
    }

    void HandleGameStart()
    {
        CoroutineRunner.StartFadeOutThenDisable(gameObject, 1f);
    }

    void HandleScreenStart()
    {
        wasClicked = false;
        t = 0f;
        transform.position = startingPosition;
        CoroutineRunner.StartEnableThenFadeIn(gameObject, 1f);
    }

    Vector3 OscillatePosition(Vector3 basePos, float time, float amp, float period)
    {
        float p = Mathf.Max(0.0001f, period);
        float phase = (time / p) * Mathf.PI * 2f;
        return new Vector3(basePos.x, startingPosition.y + Mathf.Sin(phase) * amp, basePos.z);
    }

    void Update()
    {
        if (!wasClicked && Input.GetMouseButtonDown(0))
        {
            CoroutineRunner.ChangeStateAfterDelay(1f, GameState.GameStart);
            wasClicked = true;
        }

        t += Time.unscaledDeltaTime;
        transform.position = OscillatePosition(transform.position, t, floatingAmplitude, floatingPeriod);
    }
}
