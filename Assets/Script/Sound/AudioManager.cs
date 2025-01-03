using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        public ObjectPool<AudioPlayer> AudioSourcePool { get; private set; }

        private readonly List<AudioPlayer> activeAudioSources = new();
        private AudioPlayer backgroundMusic;
        public bool isVibrationOn;
        public bool isSoundOn;

        public AudioClip buttonClickSound;
        public AudioClip mainMenuMusic;
        public AudioSource pickBox;
        public AudioSource coffeeInBox;

        [SerializeField]AudioSource bgmusic;
        [SerializeField]AudioSource GameWin;
        [SerializeField]AudioSource GameLoss;
        //private AudioSource mainMenuMusicAudioSource;

        public void DisableBgMusicOnGP()
        {
            bgmusic.gameObject.SetActive(false);// = false;
        }

        public void EnableBgMusicOnGP()
        {
            bgmusic.gameObject.SetActive(true);
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);

                AudioSourcePool = new ObjectPool<AudioPlayer>(
                    () =>
                    {
                        var audioSource = new GameObject("Audio Source", typeof(AudioPlayer));
                        audioSource.transform.SetParent(transform);
                        return audioSource.GetComponent<AudioPlayer>();
                    },
                    audioSource =>
                    {
                        audioSource.gameObject.SetActive(true);
                        activeAudioSources.Add(audioSource);
                    },
                    audioSource =>
                    {
                        audioSource.gameObject.SetActive(false);
                        activeAudioSources.Remove(audioSource);
                    },
                    audioSource => { Destroy(audioSource.gameObject); },
                    true, 50, 100
                );

                
                    //mainMenuMusicAudioSource = new GameObject("Audio Source", typeof(AudioPlayer)).GetComponent<AudioSource>();
                    //mainMenuMusicAudioSource.transform.SetParent(transform);
                    //mainMenuMusicAudioSource = audioSource.GetComponent<AudioPlayer>();
                
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            //backgroundMusic = AudioSourcePool.Get();
            //PlayBackgroundMusic(mainMenuMusic);

            // Apply saved preferences
            isSoundOn = Prefs.SoundToggle == 1;
            isVibrationOn = Prefs.VibrationToggle == 1;

            if (Prefs.MuteToggle == 1)
            {
                UnmuteBtn();
            }
            else
            {
                MuteBtn();
            }
        }

        public void MuteBtn()
        {
            Prefs.MuteToggle = 0;
            isSoundOn = false;
            bgmusic.enabled = false;
            
/*
            // Stop all active sounds and release them back to the pool
            var sourcesToRelease = new List<AudioPlayer>(activeAudioSources);

            foreach (var audioSource in sourcesToRelease)
            {
                if (audioSource != null && audioSource.gameObject.activeSelf)
                {
                    //audioSource.Stop();
                    audioSource.enabled = false;
                    AudioSourcePool.Release(audioSource);
                }
            }

            activeAudioSources.Clear();*/
        }

        public void StopBgMusic()
        {
            if (isSoundOn)
            {
                bgmusic.enabled = false;
            }
        }

        public void PlayBgMusic()
        {
            if (isSoundOn)
            {
                bgmusic.enabled = true;
            }
        }

        public void clickOnBoxPick()
        {
            if (isSoundOn)
            {
                pickBox.Play();
            }
        }

        public void CoffeeInBox()
        {
            if (isSoundOn)
            {
                coffeeInBox.Play();
            }
        }

        public void PlayGameWin()
        {
            if (isSoundOn)
            {
                GameWin.Play();
            }
        }

        public void playGameLoss()
        {
            if (isSoundOn)
            {
                GameLoss.Play();
            }
        }

        public void UnmuteBtn()
        {
            Prefs.MuteToggle = 1;
            isSoundOn = true;
            bgmusic.enabled = true;
            /*if (mainMenuMusic != null && backgroundMusic != null)
            {
                PlayBackgroundMusic(mainMenuMusic);
            }*/
        }

        public void VibrationOn()
        {
            isVibrationOn = true;
            Prefs.VibrationToggle = 1;
            Handheld.Vibrate();
        }

        public void VibrationOff()
        {
            isVibrationOn = false;
            Prefs.VibrationToggle = 0;
        }

        public void SoundOn()
        {
            isSoundOn = true;
            Prefs.SoundToggle = 1;
        }

        public void SoundOff()
        {
            isSoundOn = false;
            Prefs.SoundToggle = 0;
        }

        public void OnClickPlaySound()
        {
            if (isSoundOn && buttonClickSound != null)
            {
                AudioSourcePool.Get().PlaySound(buttonClickSound);
            }
            else
            {
                Debug.LogWarning("Button click sound not set!");
            }
        }

        public AudioPlayer PlaySound(AudioClip clip, bool loop = false)
        {
            if (isSoundOn && clip != null)
            {
                var audioSource = AudioSourcePool.Get();
                if (loop)
                {
                    audioSource.PlayLoopSound(clip);
                }
                else
                {
                    audioSource.PlaySound(clip);
                }
                return audioSource;
            }
            return null;
        }

        public void PlayBackgroundMusic(AudioClip clip)
        {
            if (clip == null) return;

            if (backgroundMusic != null)
            {
                backgroundMusic.enabled = false;
                AudioSourcePool.Release(backgroundMusic);
            }

            backgroundMusic = AudioSourcePool.Get();
            backgroundMusic.PlayLoopSound(clip);
        }
    }
}