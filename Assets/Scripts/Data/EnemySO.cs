using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemies/Enemy Data")]
public class EnemySO : ScriptableObject
{
    public float detectionRange;
    public float damagePoints;
}