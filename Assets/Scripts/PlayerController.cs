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
    public Transform explosionSpawnPoint;
    public GameObject explosionVFX;

    private bool isMovingLeft = false;
    private bool isMovingRight = false;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        float horizontalMovement = 0f;
        if (isMovingLeft)
            horizontalMovement = -1f;
        else if (isMovingRight)
            horizontalMovement = 1f;

        horizontalMovement *= horizontalSpeed * Time.deltaTime;
        
        float newXPosition = Mathf.Clamp(transform.position.x + horizontalMovement, minXPosition, maxXPosition);
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
    }

    public void OnLeftButtonDown()
    {
        isMovingLeft = true;
    }

    public void OnLeftButtonUp()
    {
        isMovingLeft = false;
    }

    public void OnRightButtonDown()
    {
        isMovingRight = true;
    }

    public void OnRightButtonUp()
    {
        isMovingRight = false;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            GameObject explosion = Instantiate(explosionVFX, explosionSpawnPoint.transform.position, Quaternion.identity);
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
            OnGameOver.Raise();
            gameObject.SetActive(false);
        }
    }
}