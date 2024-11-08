using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Audio
{
    public class UiSoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _auidoSource;

        public void Play() =>
            _auidoSource.Play();

        public class Factory : PlaceholderFactory<string, UniTask<UiSoundPlayer>>
        {
        }
    }
}
