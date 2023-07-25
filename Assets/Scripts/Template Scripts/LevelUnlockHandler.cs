using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUnlockHandler : MonoBehaviour
{

    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Transform LevelScrollView;
    int unlockedLevelsNumber;
    public int numberOFLevels;
    public SceneLoader loadScene;
    [SerializeField] RectTransform fader;
   
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("levelsUnlocked"))
        {
            PlayerPrefs.SetInt("levelsUnlocked", 1);
        }

        unlockedLevelsNumber = PlayerPrefs.GetInt("levelsUnlocked");


        for(int i=0; i <numberOFLevels ; i++) 
        {
            int index = i;
            
           GameObject clone = Instantiate(buttonPrefab, LevelScrollView);
           clone.GetComponentInChildren<TextMeshProUGUI>().text = GameStateManager.LevelManager.levels[index].levelName;
            clone.GetComponent<Button>().interactable = GameStateManager.LevelManager.levels[index].levelUnlocked;
            clone.GetComponent<Button>().onClick.AddListener(() => loadScene.LoadLevel(GameStateManager.LevelManager.levels[index].levelNumber));

        }



     

    }

    // Update is called once per frame
    void Update()
    {
        unlockedLevelsNumber = PlayerPrefs.GetInt("levelsUnlocked");

        for(int i = 0;i < unlockedLevelsNumber; i++)
        {

          //  buttonPrefab.interactable = true;
        }
    }

  


}
