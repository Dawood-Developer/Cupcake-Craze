using UnityEngine;

namespace Assets.Scripts.Audio
{
    public static class Prefs
    {
        public static int VibrationToggle
        {
            get => PlayerPrefs.GetInt(nameof(VibrationToggle), 1);
            set => PlayerPrefs.SetInt(nameof(VibrationToggle), value);
        }

        public static int SoundToggle
        {
            get => PlayerPrefs.GetInt(nameof(SoundToggle), 1); // Default to sound on
            set => PlayerPrefs.SetInt(nameof(SoundToggle), value);
        }

        public static int MuteToggle
        {
            get => PlayerPrefs.GetInt(nameof(MuteToggle), 1);
            set => PlayerPrefs.SetInt(nameof(MuteToggle), value);
        }

        public static int isGameRestarting
        {
            get => PlayerPrefs.GetInt(nameof(isGameRestarting), 0);
            set => PlayerPrefs.SetInt(nameof(isGameRestarting), value);
        }
        public static int money
        {
            get => PlayerPrefs.GetInt(nameof(money), 1);
            set => PlayerPrefs.SetInt(nameof(money), value);
        }
        public static int level
        {
            get => PlayerPrefs.GetInt(nameof(level), 0);
            set => PlayerPrefs.SetInt(nameof(level), value);
        }
        public static int levelTxt
        {
            get => PlayerPrefs.GetInt(nameof(levelTxt), 1);
            set => PlayerPrefs.SetInt(nameof(levelTxt), value);
        }

    }
}
