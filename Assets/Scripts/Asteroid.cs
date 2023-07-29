using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float detectionRange = 0.5f;
    public float moveSpeed = 2f;
    public Transform playerTransform;

    private Vector3 targetPosition;
    private bool isMovingUp = false;

    private void Start()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        targetPosition = transform.position;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

       if (distanceToPlayer < detectionRange)
        {
            isMovingUp = true;
        }

        if (isMovingUp)
        {
            targetPosition.y = 0f;
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}
