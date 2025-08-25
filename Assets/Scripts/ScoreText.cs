using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    TMP_Text scoreTMP;

    void Awake()
    {
        scoreTMP = GetComponent<TextMeshProUGUI>();
        if (GameManager.Instance) scoreTMP.text = GameManager.Instance.Score.ToString();
        GameManager.OnScoreChanged += UpdateScore;
        GameManager.OnGameOver += HandleGameOver;
        GameManager.OnGameStart += HandleGameStart;
    }

    void OnDestroy()
    {
        GameManager.OnScoreChanged -= UpdateScore;
        GameManager.OnGameOver -= HandleGameOver;
        GameManager.OnGameStart -= HandleGameStart;
    }

    void HandleGameStart() {
        CoroutineRunner.StartEnableThenFadeIn(scoreTMP);
        GameManager.Instance.ResetScore();
    }

    void HandleGameOver() => CoroutineRunner.StartFadeOutThenDisable(scoreTMP);

    void Start()
    {
        UpdateScore(0);
        gameObject.SetActive(false);
    }

    void OnEnable()
    {

    }

    void UpdateScore(int newScore)
    {
        string scoreAsSprites = "";
        foreach (char digit in newScore.ToString())
        {
            scoreAsSprites += $"<sprite name=\"{digit}\"/>";
        }
        scoreTMP.text = scoreAsSprites;
    }
}
