using UnityEngine;

public class PipesSpawner : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] GameObject pipesPrefab;

    [Header("Spawn Timing")]
    [SerializeField] float spawnsPerSecond = 1f;
    float duration;
    float spawnDuration;

    public void spawnPipes()
    {
        Instantiate(pipesPrefab, transform);
    }

    void Start()
    {
        spawnDuration = 1 / spawnsPerSecond;
        duration = spawnDuration;
    }

    void Update()
    {
        if (duration >= spawnDuration)
        {
            duration = 0;
            spawnPipes();
        }
        duration += Time.deltaTime;
    }
}
