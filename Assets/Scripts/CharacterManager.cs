using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] List<CharacterScriptableObject> charObjects;
     [SerializeField] TextMeshProUGUI charDescription;
     [SerializeField] TextMeshProUGUI charName;
     [SerializeField] TextMeshProUGUI charSpeed;
     [SerializeField] TextMeshProUGUI charStrength;
      [SerializeField] Image charImg;
    [SerializeField] int charIndex;
    [SerializeField] TextMeshProUGUI coinsText;
   
    public delegate void ShowSOEventHandler();
    public event ShowSOEventHandler OnShowSO;
    

    [SerializeField]private Button nextButton;
    [SerializeField]private Button prevButton;
    [SerializeField] private Button unlockButton;


    void Start()
    {
        OnShowSO += ShowSO;
        
        CheckButtonAvailability();
        OnShowSO?.Invoke();
      
        foreach(CharacterScriptableObject character in charObjects)
        {
            if(character.price==0)
            {
                character.isUnlocked = true;
            }
            else
            {
               if(PlayerPrefs.GetInt(character.name,0)==0)
                {
                    character.isUnlocked = false;
                }
                else
                {
                    character.isUnlocked = true;
                }
            }
        }
        UpdateUI();
    }

    public void NextCharacter()
    {
        if (charIndex < charObjects.Count - 1)
        {
            charIndex++;
            CheckButtonAvailability();
            OnShowSO?.Invoke();
        }
        UpdateUI();
    }


    public void PreviousCharacter()
    {
        if (charIndex > 0)
        {
            charIndex--;
            CheckButtonAvailability();
            OnShowSO?.Invoke();
        }
             UpdateUI();

    }


    public void SelectCharacter()
    {
        PlayerPrefs.SetInt("playerIndex", charIndex);
    }

    private void ShowSO()
    {
        charDescription.text = "<b>Description</b>\n"+ charObjects[charIndex].Description;
        charImg.sprite = charObjects[charIndex].Img;
        charName.text =charObjects[charIndex].Name;
        charSpeed.text = "<b>Speed</b>: "+charObjects[charIndex].Speed.ToString();
        charStrength.text = "<b>Strength</b>:"+charObjects[charIndex].Strength.ToString();
    }

    private void CheckButtonAvailability()
    {
        if (charIndex == 0)
        {
            prevButton.interactable = false;
            
        }
        else if (charIndex == charObjects.Count - 1)
        {
            nextButton.interactable = false;
            }
        else
        {
            prevButton.interactable = true;
            nextButton.interactable = true;
        }
    }

    public void UpdateUI()
    {
        coinsText.text = "Price: " + PlayerPrefs.GetInt("NumberOfCoins", 0);
        if (charObjects[charIndex].isUnlocked == true)
        {
            unlockButton.gameObject.SetActive(false);
        }
        else
        {
            unlockButton.GetComponentInChildren<TextMeshProUGUI>().text = "Price: " + charObjects[charIndex].price;
            if ((PlayerPrefs.GetInt("NumberOfCoins") < charObjects[charIndex].price))
            {
                unlockButton.gameObject.SetActive(true);
                unlockButton.interactable = false;
            }
            else
            {
                unlockButton.gameObject.SetActive(true);
                unlockButton.interactable = true;
            }
        }
    }

    public void Unlock()
    {
        int coins = PlayerPrefs.GetInt("NumberOfCoins", 0);
        int price = charObjects[charIndex].price;
        PlayerPrefs.SetInt("NumberOfCoins", coins - price);
        PlayerPrefs.SetInt(charObjects[charIndex].name, 1);
        charObjects[charIndex].isUnlocked = true;
        UpdateUI();
    }
}
