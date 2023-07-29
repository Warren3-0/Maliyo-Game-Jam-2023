using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float horizontalSpeed = 3f;
    public float minXPosition = -0.6f;
    public float maxXPosition = 0.12f;

    private void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        float horizontalInput = Input.GetAxis("Horizontal");

        float horizontalMovement = horizontalInput * horizontalSpeed * Time.deltaTime;

        float newXPosition = Mathf.Clamp(transform.position.x + horizontalMovement, minXPosition, maxXPosition);

        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
    }
}
