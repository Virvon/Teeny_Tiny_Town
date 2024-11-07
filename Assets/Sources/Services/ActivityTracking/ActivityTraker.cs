using Agava.WebUtility;
using System;
using UnityEngine.Audio;

namespace Assets.Sources.Services.ActivityTracking
{
    public class ActivityTraker : IDisposable
    {
        private const string MasterMixer = "Master";
        private const int MutedSoundVolume = -80;
        private const int NormalSoundVolume = 0;

        private AudioMixer _audioMixer;

        public ActivityTraker(AudioMixer audioMixer)
        {
            _audioMixer = audioMixer;

            WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;
        }

        public void Dispose() =>
            WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;

        private void OnInBackgroundChange(bool inBackground) =>
            _audioMixer.SetFloat(MasterMixer, inBackground ? MutedSoundVolume : NormalSoundVolume);
    }
}
