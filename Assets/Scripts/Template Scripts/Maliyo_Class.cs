using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Date updated: " + DateTime.Now.ToString("MM/dd/yyyy")
[System.Serializable]
public class SpawnPoint
{
    public float x;
    public float y;
    public float z;
}

public class Maliyo_Class :MonoBehaviour
{
    public Vector3 rotationSpeed;
    public Space rotationSpace;
    public void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, rotationSpace);
    }
}