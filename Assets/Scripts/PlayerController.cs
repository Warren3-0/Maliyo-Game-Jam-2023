using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float horizontalSpeed = 3f;
    public float minXPosition = -0.6f;
    public float maxXPosition = 0.12f;
    private float currentHealth;
    public float maxHealth = 100f;
    public GameEvent OnGameOver;
    public Slider healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        float horizontalInput = Input.GetAxis("Horizontal");

        float horizontalMovement = horizontalInput * horizontalSpeed * Time.deltaTime;

        float newXPosition = Mathf.Clamp(transform.position.x + horizontalMovement, minXPosition, maxXPosition);

        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            OnGameOver.Raise();
        }
    }
}
