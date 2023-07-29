using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] obstaclePrefabs;
    public float startDelay = 2.0f;
    public float spawnInterval = 2.0f;
    public float spawnOffsetZ = 5f;
    public float minXspawnPosition = -0.6f;
    public float maxXspawnPosition = 0.12f;

    private Transform playerTransform;
    private Camera mainCamera;
    public Transform spawnedObjectsParent;
    [SerializeField] private float sphereRadius = 0.5f;
    [SerializeField] private float differenceRadius = 0.1f;

    public bool canSpawn = true;

    public static SpawnManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        mainCamera = Camera.main;
        canSpawn = true;
        StartCoroutine(SpawnObstaclesAndEnemies());
    }

    private IEnumerator SpawnObstaclesAndEnemies()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            bool spawnEnemy = Random.Range(0f, 1f) > 0.5f;
            bool spawnObstacle = Random.Range(0f, 1f) > 0.5f;

            if (spawnEnemy && canSpawn)
            {
                int randomIndex = Random.Range(0, enemyPrefabs.Length);
                GameObject enemyPrefab = enemyPrefabs[randomIndex];

                EnemyContainer enemy = enemyPrefab.GetComponent<EnemyContainer>();

                Vector3 enemySpawnPosition = new Vector3();

                if (enemy.enemyCount == 1)
                    enemySpawnPosition = GenerateRandomSpawnPosition();
                else
                    enemySpawnPosition = GenerateEnemyShipSpawnPosition();
                
                GameObject enemyGO = Instantiate(enemyPrefab, enemySpawnPosition, Quaternion.identity);
                GameManager.Instance.SetEnemies(enemyGO);

                if (enemySpawnPosition == Vector3.zero)
                {
                    Destroy(enemyGO);
                }
                enemyGO.transform.SetParent(spawnedObjectsParent);
            }

            if (spawnObstacle)
            {
                GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
                Vector3 obstacleSpawnPosition = GenerateRandomSpawnPosition();
                GameObject obstacleGO = Instantiate(obstaclePrefab, obstacleSpawnPosition, Quaternion.identity);
                obstacleGO.transform.SetParent(spawnedObjectsParent);
                if (obstacleSpawnPosition == Vector3.zero)
                {
                    Destroy(obstacleGO);
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector3 GenerateRandomSpawnPosition()
    {
        int maxAttempts = 15;
        int currentAttempts = 0;

        Vector3 spawnPosition = Vector3.zero;

        while (currentAttempts < maxAttempts && spawnPosition == Vector3.zero)
        {
            float z = playerTransform.position.z + spawnOffsetZ;
            float x = Random.Range(minXspawnPosition, maxXspawnPosition);

            Vector3 tempSpawnPosition = new Vector3(x, 0, z);

            if (!IsPositionOccupied(tempSpawnPosition))
            {
                spawnPosition = tempSpawnPosition;
            }
            else
                Debug.Log("Position Occupied");
            
            currentAttempts++;
        }

        if (spawnPosition == Vector3.zero)
        {
            Debug.Log("Failed to find a valid spawn position after " + maxAttempts + " attempts.");
        }

        return spawnPosition;
    }

    private Vector3 GenerateEnemyShipSpawnPosition()
    {
        int maxAttempts = 15;
        int currentAttempts = 0;

        Vector3 spawnPosition = Vector3.zero;

        while (currentAttempts < maxAttempts && spawnPosition == Vector3.zero)
        {
            float z = playerTransform.position.z + spawnOffsetZ;
            float x = Random.Range(minXspawnPosition, maxXspawnPosition);

            Vector3 tempSpawnPosition = new Vector3(0, 0, z);

            if (!IsPositionOccupied(tempSpawnPosition))
            {
                spawnPosition = tempSpawnPosition;
            }
            else
                Debug.Log("Position Occupied");
            
            currentAttempts++;
        }

        if (spawnPosition == Vector3.zero)
        {
            Debug.Log("Failed to find a valid spawn position after " + maxAttempts + " attempts.");
        }

        return spawnPosition;
    }

    private bool IsPositionOccupied(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, sphereRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Obstacle") ||
                collider.CompareTag("Enemy"))
            {
                return true;
            }
        }

        return false;
    }
}