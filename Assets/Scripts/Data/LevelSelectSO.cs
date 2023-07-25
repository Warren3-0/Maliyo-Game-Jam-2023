using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "LevelScriptableObject", menuName = "LevelScriptableObject")]
public class LevelSelectSO : ScriptableObject
{
    public string levelName;
    public int numberOfEnemies;
    public bool levelUnlocked;
    public int levelNumber;
}
