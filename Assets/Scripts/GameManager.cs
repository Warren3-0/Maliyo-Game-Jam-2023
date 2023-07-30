using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameManager : MonoBehaviour
{
    //private List<GameObject> currentSpawnedEnemies = new List<GameObject>();

    public int currentEnemyShipsCount = 0;

    public static GameManager Instance;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI enemyDestroyedText;
    private float distanceTraveled = 0f;
    private int lastDistance;
    public Transform player;
    private int enemyDestroyed = 0;

    private void Awake()
    {
        Instance = this;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        currentEnemyShipsCount = 0;
        enemyDestroyed = 0;
    }

    private void FixedUpdate()
    {
        lastDistance = (int)distanceTraveled;
		distanceTraveled += (Time.deltaTime);
		if (lastDistance== (int)distanceTraveled) 
		{
			distanceText.text = distanceTraveled.ToString("0");
		}
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
        enemyDestroyed++;
        enemyDestroyedText.text = enemyDestroyed.ToString();
        if (currentEnemyShipsCount <= 2)
        {
            SpawnManager.Instance.canSpawn = true;
        }
    }
}