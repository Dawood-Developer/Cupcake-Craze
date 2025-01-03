using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Audio
{
    [RequireComponent(typeof(Button))]
    public class ButtonUiAudioPlayer : MonoBehaviour
    {
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            button.onClick.AddListener(OnClickPlaySound);
        }
        private void OnDisable()
        {
            button.onClick.RemoveListener(OnClickPlaySound);
        }
        private void OnClickPlaySound()
        {
            AudioManager.instance?.OnClickPlaySound();
        }
    }
}