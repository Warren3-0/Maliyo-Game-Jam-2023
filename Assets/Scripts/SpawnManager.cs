using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] obstaclePrefabs;
    public float startDelay = 2.0f;
    public float spawnInterval = 2.0f;
    public float spawnOffsetX = 2f;
    public float maxYSpawnPosition = 5f;
    public float minYSpawnPosition = -5f;
    public float despawnDelay = 2.0f;
    public float waitDelay = 2.0f;

    private Transform playerTransform;
    private Camera mainCamera;

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
            bool spawnObstacle = Random.Range(0f, 1f) > 0.7f;

            if (spawnEnemy)
            {
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                Vector3 enemySpawnPosition = GenerateRandomSpawnPosition();
                GameObject enemy = Instantiate(enemyPrefab, enemySpawnPosition, Quaternion.identity);
                StartCoroutine(DestroyOffscreenObject(enemy));
            }

            if (spawnObstacle)
            {
                GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
                Vector3 obstacleSpawnPosition = GenerateRandomSpawnPosition();
                GameObject obstacle = Instantiate(obstaclePrefab, obstacleSpawnPosition, Quaternion.identity);
                StartCoroutine(DestroyOffscreenObject(obstacle));
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector3 GenerateRandomSpawnPosition()
    {
        float y = Random.Range(minYSpawnPosition, maxYSpawnPosition);
        float playerX = playerTransform.position.x;
        float x = playerX + spawnOffsetX;

        return new Vector3(x, y, 0f);
    }

    private IEnumerator DestroyOffscreenObject(GameObject obj)
    {
        yield return new WaitForSeconds(waitDelay); // Wait for the next frame to ensure object position is up to date

        while (IsOffscreen(obj))
        {
            yield return null;
        }

        yield return new WaitForSeconds(despawnDelay);
        Destroy(obj);
    }

    private bool IsOffscreen(GameObject obj)
    {
        Vector3 screenPos = mainCamera.WorldToViewportPoint(obj.transform.position);
        return (screenPos.x < 0f || screenPos.x > 1f || screenPos.y < 0f || screenPos.y > 1f);
    }
}