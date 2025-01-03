using UnityEngine;

namespace Assets.Scripts.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
            Invoke(nameof(ReturnToPool), clip.length);
        }

        private void ReturnToPool()
        {
            audioSource.Stop();
            AudioManager.instance?.AudioSourcePool.Release(this);
        }
        public void PlayLoopSound(AudioClip clip)
        {
            audioSource.loop = true;
            PlaySound(clip);
        }
    }
}