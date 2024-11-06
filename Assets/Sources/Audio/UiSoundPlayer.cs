using UnityEngine;

namespace Assets.Sources.Audio
{
    public class UiSoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _auidoSource;

        public void Play() =>
            _auidoSource.Play();
    }
}
