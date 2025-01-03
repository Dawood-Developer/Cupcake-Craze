using Assets.Scripts.Audio;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkButton : MonoBehaviour
{

    [SerializeField] Perks perk;
    Button button;
    Text text;

    private void OnEnable()
    {
        PerkPanel.onPerkUsed += DecreasePerkCount;
    }

    private void OnDisable()
    {
        PerkPanel.onPerkUsed -= DecreasePerkCount;
    }

    private void Start()
    {
        SetRefrences();
        UpdatePerksCount();
    }

    void UpdatePerksCount()
    {
        text.text = PlayerPrefs.GetInt(perk.ToString(), 0).ToString();
    }
    [Button]
    void DecreasePerkCount(string perkName)
    {
        if(PlayerPrefs.GetInt(perkName) > 0)
        PlayerPrefs.SetInt(perkName, PlayerPrefs.GetInt(perkName) - 1);
        UpdatePerksCount();
    }
    [Button]
    void IncreasePerkCount(int amount)
    {
        PlayerPrefs.SetInt(perk.ToString(), PlayerPrefs.GetInt(perk.ToString()) + amount);
        UpdatePerksCount();
    }

    void SetRefrences()
    {
        text = GetComponentInChildren<Text>();
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            GameManager.instance.DoPerk(perk.ToString());
        });
    }

}

    [SerializeField]public enum Perks { SortAllCupsOnChain, ShuffleAllBoxesOnPosition, Sort1Box, SortRandom3Boxes}