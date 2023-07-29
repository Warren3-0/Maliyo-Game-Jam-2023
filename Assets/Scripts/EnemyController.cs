using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float forwardSpeed = 5f;
    public float backwardSpeed = 2f;

    public float playerDetectionDistance = 1f;
    private Transform playerTransform;
    private bool isMovingForward = true;

    private float health = 50f;

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

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        GameManager.Instance.RemoveEnemy();
    }
}