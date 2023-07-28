using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public float forwardSpeed = 5f;
    public float backwardSpeed = 2f;
    public float playerDetectionDistance = 1f;

    private Transform playerTransform;
    private bool isMovingForward = true;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (playerTransform == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (isMovingForward)
        {
            if (distanceToPlayer > playerDetectionDistance)
            {
                transform.Translate(Vector3.back * backwardSpeed * Time.deltaTime);
            }
            else
            {
                isMovingForward = false;
            }
        }
        else
        {
            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
        }
    }
}