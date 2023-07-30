using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    //private List<GameObject> currentSpawnedEnemies = new List<GameObject>();

    public int currentEnemyShipsCount = 0;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentEnemyShipsCount = 0;
    }

    public void SetEnemies(GameObject enemyGO)
    {
        EnemyContainer enemyContainer = enemyGO.GetComponent<EnemyContainer>();
        currentEnemyShipsCount += enemyContainer.enemyCount;

        if (currentEnemyShipsCount >= 4)
            SpawnManager.Instance.canSpawn = false;
    }

    public void RemoveEnemy(EnemyContainer enemy)
    {
        currentEnemyShipsCount--;
        enemy.enemyCount--;
        if (currentEnemyShipsCount <= 2)
        {
            SpawnManager.Instance.canSpawn = true;
        }
    }
}