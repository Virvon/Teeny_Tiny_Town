using Assets.Sources.Audio;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI
{
    public class SoundButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private UiSoundPlayer _uiSoundPlayer;

        [Inject]
        private void Construct(UiSoundPlayer uiSoundPlayer)
        {
            _uiSoundPlayer = uiSoundPlayer;

            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDestroy() =>
            _button.onClick.RemoveListener(OnButtonClicked);

        private void OnButtonClicked() =>
            _uiSoundPlayer.Play();
    }
}
