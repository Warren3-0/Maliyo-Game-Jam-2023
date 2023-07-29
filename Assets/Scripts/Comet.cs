using UnityEngine;

public class Comet : MonoBehaviour
{
    public float speed = 10f;
    public float startYPosition = 10f;
    public Transform playerTransform;

    private Vector3 targetPosition;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, startYPosition, transform.position.z);

        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        targetPosition = playerTransform.position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.005f)
        {
            playerTransform.gameObject.GetComponent<PlayerController>().TakeDamage(5);
            Destroy(gameObject);
        }
    }
}
