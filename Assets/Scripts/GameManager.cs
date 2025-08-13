using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Refs")]
    [SerializeField] MovingForeground movingForeground;

    [Header("Game States")]
    public bool gameStart = true;
    public bool gameOver = false;

    [Header("Shared Data")]
    public int score = 0;
    public float minX = 0f;
    public float maxX = 0f;

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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
