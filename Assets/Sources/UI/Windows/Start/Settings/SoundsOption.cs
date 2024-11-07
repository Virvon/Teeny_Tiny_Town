using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Sources.UI.Windows.Start
{
    public class SoundsOption : SettingsOption
    {
        [SerializeField] AudioMixer _mixer;

        protected override void OnToggleValueChanged(bool value)
        {
            _mixer.SetFloat("SoundsVolume", value ? 0 : -80);
            PersistentProgressService.Progress.SettingsData.ChangeSoundsActive(value);
        }

        protected override bool SetUpToggle()
        {
            _mixer.SetFloat("SoundsVolume", PersistentProgressService.Progress.SettingsData.IsSoundsOn ? 0 : -80);
            return PersistentProgressService.Progress.SettingsData.IsSoundsOn;
        }
    }
}
