using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
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

        Vector3 direction = playerTransform.position - transform.position;

        float distanceToPlayer = direction.magnitude;

        if (isMovingForward)
        {
            if (distanceToPlayer > playerDetectionDistance)
            {
                transform.Translate(-transform.forward * backwardSpeed * Time.deltaTime);
            }
            else
            {
                isMovingForward = false;
            }
        }
        else
        {
            transform.Translate(transform.forward * forwardSpeed * Time.deltaTime);
        }
    }
}