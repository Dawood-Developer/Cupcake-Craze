using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Audio;
using Sirenix.OdinInspector;

public class WheelManager : MonoBehaviour {

    //Creates the wheel
    SpinWheel wheel = new SpinWheel(8);
    public GameObject TapButton;
    public GameObject[] CloseDuringSpinGO;
    public GameObject win;
    public Text winT;

    public PersistentTimer timerManager;
    public int cooldown;
    public static bool isTimerComplete = true;

    private void OnEnable()
    {
        SetSpinButtonState(isTimerComplete);
    }
    void Start() {

        wheel.setWheel(gameObject);

        //Sets the callback
        wheel.AddCallback((index) => {
            switch (index)
            {
                case 1:
                    MainMenu.UpdateMoney(100);
                    win.SetActive(true);
                    winT.text = "100";
                    break;
                case 2:
                    Prefs.money += 500;
                    MainMenu.UpdateMoney(500);
                    win.SetActive(true);
                    winT.text = "500";
                    break;
                case 3:
                    Prefs.money += 100;
                    MainMenu.UpdateMoney(100);
                    win.SetActive(true);
                    winT.text = "100";
                    break;
                case 4:
                    Prefs.money += 100;
                    MainMenu.UpdateMoney(100);
                    win.SetActive(true);
                    winT.text = "100";
                    break;
                case 5:
                    Prefs.money += 200;
                    MainMenu.UpdateMoney(200);
                    win.SetActive(true);
                    winT.text = "200";
                    break;
                case 6:
                    Prefs.money += 100;
                    MainMenu.UpdateMoney(100);

                    win.SetActive(true);
                    winT.text = "100";
                    break;
                case 7:
                    Prefs.money += 200;
                    MainMenu.UpdateMoney(200);
                    win.SetActive(true);
                    winT.text = "200";
                    break;
                case 8:
                    MainMenu.UpdateMoney(300);
                    win.SetActive(true);
                    winT.text = "300";
                    break;
            }
        });
	}

    public void Spin()
    {
        if (Prefs.money >= 300)
        {
            MainMenu.UpdateMoney(-300);
            StartCoroutine(wheel.StartNewRun());
            SetSpinButtonState(false);
            isTimerComplete = false;
            timerManager.StartTimer(cooldown);
            StartCoroutine(EnableCloseButton());
        }
    }

    public void SetSpinButtonState(bool update)
    {
        TapButton.SetActive(update);
    }
    private IEnumerator EnableCloseButton()
    {
        CloseDuringSpinGO[0].SetActive(false);  
        CloseDuringSpinGO[1].SetActive(false);  

        yield return new WaitForSeconds(8);

        CloseDuringSpinGO[0].SetActive(true);
        CloseDuringSpinGO[1].SetActive(true);
    }

}

