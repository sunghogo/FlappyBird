using UnityEngine;

public class Pipes : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] float speed = 1f;
    [SerializeField] float height = 0f;
    [SerializeField, Range(-1f, 1.5f)] float minHeight = 0f;
    [SerializeField, Range(-1f, 1.5f)] float maxHeight = 1f;


    public void RandomizePipesHeight()
    {
        height = Random.Range(minHeight, maxHeight);
        transform.Translate(Vector2.up * height);
    }

    public void Move(Vector2 impulse)
    {
        transform.Translate(impulse);
    }

    void Awake()
    {
        GameManager.OnGameOver += HandleGameOver;
    }

    void OnDestroy()
    {
        GameManager.OnGameOver -= HandleGameOver;
    }

    void HandleGameOver() => CoroutineRunner.StartFadeOutThenDisable(gameObject);

    void Start()
    {
        RandomizePipesHeight();
    }

    void Update()
    {
        if (transform.position.x < GameManager.Instance.minX)
        {
            Destroy(gameObject);
        }
        
        transform.Translate(Vector2.left * Time.deltaTime * speed);
    }
}
