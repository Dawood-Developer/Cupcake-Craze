using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelHandler : MonoBehaviour
{
    #region Singleton
    public static PanelHandler instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); 
        }
    }
    #endregion

    [Header("Panel Refrence")]
    public GameObject MainMenu;
    public GameObject LoadingScreen;

    [Header("Canvas")]
    public GameObject MainMenuCanvas;
    public GameObject GameCanvas;

    [Button]
    public void SetRefrence()
    {
        MainMenu = transform.Find(nameof(MainMenu)).gameObject;
        LoadingScreen = transform.Find(nameof(LoadingScreen)).gameObject;
        MainMenuCanvas = gameObject;
        GameCanvas = transform.parent.Find(nameof(GameCanvas)).gameObject;

    }

}
