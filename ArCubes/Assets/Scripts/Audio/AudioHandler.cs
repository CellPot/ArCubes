using UnityEngine;

namespace Audio
{
    public class AudioHandler : MonoBehaviour
    {
        [SerializeField] private AudioSource collectSource;
        [SerializeField] private AudioClip collectClip;

        private void Awake()
        {
            collectSource.clip = collectClip;
        }

        public void PlayCoinSound()
        {
            collectSource.Play();
        }
    }
}
