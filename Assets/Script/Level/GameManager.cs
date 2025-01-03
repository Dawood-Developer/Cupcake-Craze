using Assets.Scripts.Audio;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public delegate void OnCoinOrPerkUpdateDelegate();

    // Declare an event using the delegate
    public static event OnCoinOrPerkUpdateDelegate OnCoinOrPerkUpdate;

    [Title("Materials"), InlineProperty]
    [Required, Tooltip("Materials should be added in the same order as the BoxColor enum.")]
    public Sprite[] sprite;

    [SerializeField] GameObject gameEndPanel;
    [SerializeField] GameObject gameWinPanel;
    [SerializeField] Text winPriceTxt;
    [SerializeField] Text levelCountTxt;
    [SerializeField] int winPrice;
    [SerializeField] GameObject missionFailPanel;
    [SerializeField] LevelManager levelManager;
    [SerializeField] CupHandler cupHandler;
    [SerializeField] PerkPanel perkPanel;
    [SerializeField] GameObject SettingPanel;
    [SerializeField] Slot[] slots;

    public Animator anim;
    public LevelManager lvlManager()
    {
        return levelManager;
    }
    public MainMenu mainMenu;
    public bool isAlreadyOpenAnyPanel = true;
    public void UpdatePerks()
    {
        OnCoinOrPerkUpdate?.Invoke();
    }

    private void Awake()
    {
        instance = this;
        AudioManager.instance.PlayBgMusic();
    }

    public void ShowInststatialAd()
    {
        //AdManager.instance.ShowInterstitital();
    }

    public void GameLose(bool isSlotBook)
    {
        anim.Play("RevivePanelOpen");
        gameEndPanel.SetActive(true);
        //gameEndStatus.text = status;
    }

    public void GameWin()
    {
        IsAnyPanelOpen(true);
        gameWinPanel.SetActive(true);
        winPrice *= Prefs.levelTxt;
        levelCountTxt.text = "Level " + Prefs.levelTxt.ToString();
        winPriceTxt.text = winPrice.ToString();
        GoToNextLevel();
    }
    private void GoToNextLevel()
    {
        Prefs.levelTxt++;

        Prefs.level++;

        if (Prefs.level >= levelManager.levels.Count)
        {
            Prefs.level = 0;
        }

        levelManager.currentLevelIndex = Prefs.level;
    }

    public void ClaimWinPrize(int isDouble)
    {
        levelManager.InitNewLevel();
        gameWinPanel.SetActive(false);
        MainMenu.UpdateMoney(winPrice);
        if (isDouble > 0)
        MainMenu.UpdateMoney(winPrice);
        mainMenu.SwitchCanvas(0);
    }
    
    public void DoPerk(string perk)
    {
        Invoke(perk, 0f);
    }

    void SortAllCupsOnChain()
    {
        perkPanel.Init("SHUFFLE", sprite[0], cupHandler.SortCupsByColor,"Shuffle all boxes position",nameof(SortAllCupsOnChain));
    }

    void ShuffleAllBoxesOnPosition()
    {
        perkPanel.Init("SORT", sprite[1], levelManager.SwipeBoxesOfAllLayers, "Sort all cups on chain", nameof(ShuffleAllBoxesOnPosition));
    }

    void Sort1Box()
    {
        perkPanel.Init("CLEAR", sprite[2], () => levelManager.CheckBoxesInLayers(cupHandler.GetColorOfCurrentCupCakeOnConvare()),"Sort 1 box", nameof(Sort1Box));
    }

    void SortRandom3Boxes()
    {
        perkPanel.Init("CLEAR 3", sprite[3], () => levelManager.CheckBoxesInLayers(cupHandler.GetColorOfCurrentCupCakeOnConvare()), "Random sort 3 box", nameof(SortRandom3Boxes));
    }

    public void StartInststatialAd()
    {

    }

    public void OpenSettingPanel()
    {
        SettingPanel.SetActive(true);
    }

    public void IsAnyPanelOpen(bool update)
    {
        isAlreadyOpenAnyPanel = update;
    }
    /*    public void Restart()
        {
            SceneManager.LoadScene(1);
        }*/

    public void Home()
    {
        //        SceneManager.LoadScene(0);
        mainMenu.SwitchCanvas(0);
    }

    [Button]

    public void TryAgain()
    {
        Prefs.isGameRestarting = 1;
        SceneManager.LoadScene(0);
    }

    public void unlockSlot()
    {
        if (Prefs.money >= 80)
        {
            foreach (var item in slots)
            {
                if (item.Unlock())
                {
                    gameEndPanel.SetActive(false);
                    isAlreadyOpenAnyPanel = false;
                    MainMenu.UpdateMoney(-80);
                    return;
                    //detuct money here.
                }
            }
        }
    }
}
