using UnityEngine;

public class GameOverSprite : MonoBehaviour
{
    void Awake()
    {
        GameManager.OnGameOver += HandleGameOver;
    }

    void OnDestroy()
    {
        GameManager.OnGameOver -= HandleGameOver;
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    void HandleGameOver()
    {
        CoroutineRunner.StartEnableThenFadeIn(gameObject, 1f);
        CoroutineRunner.StartFadeOutThenDisable(gameObject, 2f, 1f);
        CoroutineRunner.ChangeStateAfterDelay(3f, GameState.StartingScreen);
    }

}
