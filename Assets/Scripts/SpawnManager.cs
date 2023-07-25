using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    int index;
    // Start is called before the first frame update
    void Start()
    {
        index = PlayerPrefs.GetInt("playerIndex");
        GameObject clone = Instantiate(playerPrefabs[index], playerPrefabs[index].transform.position,Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
