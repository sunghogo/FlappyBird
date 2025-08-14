using UnityEngine;
using System;

public enum GameState
{
    StartingScreen,
    GameStart,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static event Action<int> OnScoreChanged;
    public static event Action OnGameStart;
    public static event Action OnGameOver;
    public static event Action OnScreenStart;

    [Header("Game States")]
    [SerializeField] bool startingScreen = true;
    [SerializeField] bool gameStart = false;
    [SerializeField] bool gameOver = false;
    public bool StartingScreen
    {
        get => startingScreen;
        private set => startingScreen = value;
    }
    public bool GameStart
    {
        get => gameStart;
        private set => gameStart = value;
    }
    public bool GameOver
    {
        get => gameOver;
        private set => gameOver = value;
    }

    [Header("Shared Data")]
    public float minX = 0f;
    public float maxX = 0f;
    [field: SerializeField] public int Score { get; private set; }

    public void IncrementScore()
    {
        Score += 1;
        OnScoreChanged?.Invoke(Score);
    }

    public void ResetScore()
    {
        Score = 0;
        OnScoreChanged?.Invoke(Score);
    }

    public void StartGame()
    {
        StartingScreen = false;
        GameStart = true;
        GameOver = false;
        OnGameStart?.Invoke();
    }

    public void EndGame()
    {
        StartingScreen = false;
        GameStart = false;
        GameOver = true;
        OnGameOver?.Invoke();
    }

    public void StartScreen()
    {
        StartingScreen = true;
        GameStart = false;
        GameOver = false;
        OnScreenStart?.Invoke();
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
