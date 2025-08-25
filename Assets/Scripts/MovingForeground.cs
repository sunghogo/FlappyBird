using System.Collections.Generic;
using UnityEngine;

public class MovingForeground : MonoBehaviour
{
    [SerializeField] float leftBoundaryX = 0f, rightBoundaryX = 0f;
    [SerializeField] float speed = 2f;
    [SerializeField] List<Transform> groundPartitionsList = new List<Transform>();

    public void InitializeLists()
    {
        List<Transform> transforms = new List<Transform>();
        transforms.AddRange(GetComponentsInChildren<Transform>(false));
        transforms.Remove(transform);
        foreach (Transform obj in transforms)
        {
            if (obj.CompareTag("Ground")) groundPartitionsList.Add(obj);
         }
    }

    public void SetBoundaryLimits()
    {
        foreach (Transform partition in groundPartitionsList)
        {
            if (!partition.CompareTag("Ground")) continue;
            float x = partition.position.x;
            float width = partition.gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
            float leftX = x - width / 2;
            float rightX = x + width / 2;
            if (leftX < leftBoundaryX)
            {
                leftBoundaryX = leftX;
                GameManager.Instance.minX = leftX;
            }
            if (rightX > rightBoundaryX)
            {
                rightBoundaryX = rightX;
                GameManager.Instance.maxX = rightX;

            }
        }
    }

    public void MoveGroundPartitionsLeft()
    {
        if (groundPartitionsList.Count == 0 || !GameManager.Instance.GameStart) return;
        foreach (Transform partition in groundPartitionsList)
        {
            partition.Translate(Vector2.left * Time.fixedDeltaTime * speed);
        }
    }

    public void CheckThenLoopObjects()
    {
        if (!GameManager.Instance.GameStart) return;
        if (groundPartitionsList.Count > 0)
        {
            foreach (Transform partition in groundPartitionsList)
            {
                if (partition.position.x < leftBoundaryX)
                {
                    partition.position = new Vector3(rightBoundaryX, partition.position.y, partition.position.z);
                }
            }
        }
    }

    void Start()
    {
        InitializeLists();
        SetBoundaryLimits();
    }

    void FixedUpdate()
    {
        MoveGroundPartitionsLeft();
        CheckThenLoopObjects();
    }

}
