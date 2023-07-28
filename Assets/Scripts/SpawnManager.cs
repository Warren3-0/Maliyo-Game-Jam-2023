using System.Collections;
using UnityEngine;

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

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        mainCamera = Camera.main;
        StartCoroutine(SpawnObstaclesAndEnemies());
    }

    private IEnumerator SpawnObstaclesAndEnemies()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            bool spawnEnemy = Random.Range(0f, 1f) > 0.5f;
            bool spawnObstacle = Random.Range(0f, 1f) > 0.8f;

            if (spawnEnemy)
            {
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                Vector3 enemySpawnPosition = GenerateRandomSpawnPosition();
                GameObject enemyGO = Instantiate(enemyPrefab, enemySpawnPosition, Quaternion.identity);
                enemyGO.transform.SetParent(spawnedObjectsParent);
            }

            if (spawnObstacle)
            {
                GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
                Vector3 obstacleSpawnPosition = GenerateRandomSpawnPosition();
                GameObject obstacleGO = Instantiate(obstaclePrefab, obstacleSpawnPosition, Quaternion.identity);
                obstacleGO.transform.SetParent(spawnedObjectsParent);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector3 GenerateRandomSpawnPosition()
    {
        int maxAttempts = 10;
        int currentAttempts = 0;

        float z = playerTransform.position.z + spawnOffsetZ;
        float x = Random.Range(minXspawnPosition, maxXspawnPosition);

        Vector3 spawnPosition =  new Vector3(x, 0, z);

        while (currentAttempts < maxAttempts && IsPositionOccupied(spawnPosition))
        {
            z = playerTransform.position.z + spawnOffsetZ;
            x = Random.Range(minXspawnPosition, maxXspawnPosition);

            spawnPosition =  new Vector3(x, 0, z);
            currentAttempts++;
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