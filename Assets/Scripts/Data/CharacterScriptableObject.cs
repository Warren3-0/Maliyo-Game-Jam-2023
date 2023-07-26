using UnityEngine;
// Date updated: " + DateTime.Now.ToString("MM/dd/yyyy")
[CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "CharacterScriptableObject")]
public class CharacterScriptableObject : ScriptableObject
{
    public Sprite Img;
    public string Name;
    public string Description;
    public int Speed;
    public float Strength;
    public int price;
    public bool isUnlocked;
    public GameObject playerPrefab;
}