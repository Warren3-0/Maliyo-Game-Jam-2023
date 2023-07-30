using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float forwardSpeed = 5f;
    public float backwardSpeed = 2f;

    public float playerDetectionDistance = 1f;
    private Transform playerTransform;
    private bool isMovingForward = true;

    private float shootingInterval = 2f; 
     public GameObject laserPrefab;
    public Transform firePoint;
    public float maxHealth = 50;
    public float maneuverDistance = 0.2f;
    public float maneuverDuration = 2f;
    private bool isManeuvering = false;
    private EnemyContainer enemyContainer;
    public float avoidanceRadius = 0.15f;

    private float currentHealth;
    public Slider healthBar;
    public EnemySO enemySO;
    public Transform explosionSpawnPoint;
    public ParticleSystem muzzleFlash;


    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemyContainer = transform.parent.GetComponent<EnemyContainer>();
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
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
                StartCoroutine(ShootLasers());
                isMovingForward = false;
            }
        }
        else
        {
            transform.Translate(transform.forward * forwardSpeed * Time.deltaTime);
        }

        if (IsOverlappingWithOtherShips(transform.position * maneuverDistance))
        {
            StartCoroutine(PerformManeuver());
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.value = currentHealth;

        if (currentHealth <= 0f)
        {
            Die();
        }
        
        if (enemyContainer.enemyCount <= 2)
        {
            StartCoroutine(PerformManeuver());
        }
    }

    private void Die()
    {
        GameObject explosion = Instantiate(enemySO.explosionVFX, explosionSpawnPoint.transform.position, Quaternion.identity);
        Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration + 0.1f);
        GameManager.Instance.RemoveEnemy(enemyContainer);
        Destroy(gameObject);
    }

    private IEnumerator ShootLasers()
    {
        yield return new WaitForSeconds(0.5f);
        
        while (true)
        {
            if (playerTransform != null)
            {
                muzzleFlash.Play();
                Vector3 directionToPlayer = playerTransform.position - firePoint.position;
                GameObject laserGO = Instantiate(laserPrefab, firePoint.position, Quaternion.LookRotation(directionToPlayer));
                EnemyLaser laser = laserGO.GetComponent<EnemyLaser>();
                laser.SetDamage(enemySO.damagePoints);
                Destroy(laserGO, 2f);
            }

            yield return new WaitForSeconds(shootingInterval);
        }
    }

    
    private IEnumerator PerformManeuver()
    {
        isManeuvering = true;

        Vector3 originalPosition = transform.position;
        Vector3 direction = Vector3.right;

        for (int i = 0; i < 2; i++)
        {
            Vector3 testPosition = originalPosition + direction * maneuverDistance;

            if (!IsOverlappingWithOtherShips(testPosition) && IsWithinBounds(testPosition))
            {
                float elapsedTime = 0f;

                while (elapsedTime < maneuverDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float t = Mathf.SmoothStep(0f, 1f, elapsedTime / maneuverDuration);
                    Vector3 newPosition = transform.position;
                    newPosition.x = Mathf.Lerp(originalPosition.x, testPosition.x, t);
                    transform.position = newPosition;
                    yield return null;
                }

                break;
            }

            direction = -direction;
        }

        isManeuvering = false;
    }


    private bool IsOverlappingWithOtherShips(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, avoidanceRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy") && collider.gameObject != gameObject)
            {
                return true;
            }
        }

        return false;
    }

    private bool IsWithinBounds(Vector3 position)
    {
        return position.x >= SpawnManager.Instance.minXspawnPosition && position.x <= SpawnManager.Instance.maxXspawnPosition;
    }
}