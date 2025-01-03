using Assets.Scripts.Audio;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    PanelHandler panelHandler;

    [Header("Loading Screen")]
    [SerializeField] GameObject Loading;
    [SerializeField] Image loadingBarImage;
    [SerializeField] Text loadingCountText;

    [Header("Main Menu")]
    [SerializeField] Text levelCount;
    [SerializeField] GameObject settingPanel;

    [Header("Money")]
    [SerializeField] Text[] moneyText;
    private static Text[] staticMoneyText;
    private void OnEnable()
    {
        levelCount.text = "Level " + Prefs.levelTxt.ToString();
    }
    private void Start()
    {
        panelHandler = PanelHandler.instance;
        staticMoneyText = new Text[moneyText.Length];
        for (int i = 0; i < moneyText.Length; i++)
        {
            staticMoneyText[i] = moneyText[i];
        }
        if (Prefs.isGameRestarting == 1)
        {
            Prefs.isGameRestarting = 0;
            StartCoroutine(LoadSceneAfterDelay(3f));
        }
        else
        {
            StartCoroutine(FillLoadingBar(3f));
        }
        GetInitialValues();
    }

    void GetInitialValues()
    {
        staticMoneyText[0].text = moneyText[0].text;
        staticMoneyText[1].text = moneyText[1].text;
        UpdateMoney(0);
    }

    public IEnumerator FillLoadingBar(float duration)
    {
        Loading.SetActive(true);

        float currentTime = 0f; // Current time elapsed
        loadingBarImage.fillAmount = 0f; // Start from 0

        while (currentTime < duration)
        {
            // Increment the timer
            currentTime += Time.deltaTime;

            // Calculate progress (0 to 1)
            float progress = Mathf.Clamp01(currentTime / duration);

            // Update the fill amount of the loading bar
            loadingBarImage.fillAmount = progress;
            int percentage = Mathf.RoundToInt(progress * 100f);
            loadingCountText.text = percentage.ToString() + "%";
            yield return null; // Wait for the next frame
        }
        loadingBarImage.fillAmount = 1f;
        Loading.SetActive(false);
    }

    public void ShowSettingPanel()
    {
        settingPanel.SetActive(true);
    }


    public void OnPlayButtonClick()
    {
        StartCoroutine(LoadSceneAfterDelay(3f));
        
    }
    IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return FillLoadingBar(delay);
        SwitchCanvas(1);
        GameManager.instance.isAlreadyOpenAnyPanel = false;
    }


    public void SwitchCanvas(int i)
    {
        switch (i)
        {
            case 0:
                panelHandler.GameCanvas.SetActive(false);
                panelHandler.MainMenuCanvas.SetActive(true);
                break;
            case 1:
                panelHandler.MainMenuCanvas.SetActive(false);
                panelHandler.GameCanvas.SetActive(true);
                break;
        }
    }

    [Button]
    public static void UpdateMoney(int amount)
    {
        Prefs.money += amount;
        staticMoneyText[0].text = Prefs.money.ToString();
        staticMoneyText[1].text = Prefs.money.ToString();
    }
}
