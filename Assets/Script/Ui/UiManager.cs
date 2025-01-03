using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] Text currencyCoin;

    private void OnEnable()
    {
        GameManager.OnCoinOrPerkUpdate += UpdateCashCount;
    }

    private void OnDisable()
    {
        GameManager.OnCoinOrPerkUpdate -= UpdateCashCount;
    }


    void Start()
    {
        UpdateCashCount();
    }

    private void UpdateCashCount()
    {
        //currencyCoin.text = PlayerPrefs.GetInt("Coins",0).ToString();
    }
}
