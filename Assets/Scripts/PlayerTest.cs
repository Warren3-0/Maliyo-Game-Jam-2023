using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float verticalSpeed = 3f;
    public float minYPosition = -0.45f;
    public float maxYPosition = 0.45f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }
}