using Assets.Scripts.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PerkPanel : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Text actionName;
    [SerializeField] Image image;
    [SerializeField] Text description;

    GameManager gameManager;
    MainMenu menu;
    UnityAction unityAction;
    string unityActionName;

    public static event Action<string> onPerkUsed;
    public static event Action<int> onPerkObtained;

    private void Start()
    {
        gameManager = GameManager.instance;
        menu = gameManager.mainMenu;
    }
    public void Init(string an, Sprite sprite, UnityAction ua, string desc, string pn)
    {
        unityAction = ua;
        unityActionName = pn.ToString();
        if (PlayerPrefs.GetInt(pn) == 0)
        {
            gameManager.anim.Play("PerkPanelOpen");
            actionName.text = an;
            description.text = desc;
            image.sprite = sprite;
            image.SetNativeSize();
            panel.SetActive(true);
            GameManager.instance.isAlreadyOpenAnyPanel = true;
        }
        else 
        {
            unityAction.Invoke();
            onPerkUsed?.Invoke(unityActionName);
        }
    }

    public void PerkWithMoney()
    {
        if (Prefs.money >= 80)
        {
            MainMenu.UpdateMoney(-80);
            unityAction.Invoke();
            onPerkUsed?.Invoke(unityActionName);
            panel.SetActive(false);
            GameManager.instance.isAlreadyOpenAnyPanel = false;
        }
    }

    public void PerkWithAd()
    {
        unityAction.Invoke();
        onPerkUsed?.Invoke(unityActionName);
        panel.SetActive(false);
        GameManager.instance.isAlreadyOpenAnyPanel = false;

    }

    public void Close()
    {
        panel.SetActive(false);
    }

}