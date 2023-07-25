using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CoinManager : MonoBehaviour
{
    public int numberOfCoins;
    public TextMeshProUGUI coinsText;
    // Start is called before the first frame update
    void Start()
    {
       PlayerPrefs.SetInt("NumberOfCoins", numberOfCoins);
    }

    // Update is called once per frame
    void Update()
    {
        coinsText.text = "Coins: " + PlayerPrefs.GetInt("NumberOfCoins");
    }

    public void AddPoint()
    {
        numberOfCoins++;
        PlayerPrefs.SetInt("NumberOfCoins", numberOfCoins);
        coinsText.text = "Coins: " + PlayerPrefs.GetInt("NumberOfCoins");
    }
}
