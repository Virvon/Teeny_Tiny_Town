using Assets.Sources.Data;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.Services.SaveLoadProgress
{
    internal class SaveLoadService : ISaveLoadService
    {
        private const string Key = "Progress";

        private readonly IPersistentProgressService _progressService;

        public SaveLoadService(IPersistentProgressService progressService) =>
            _progressService = progressService;

        public PlayerProgress LoadProgress() =>
            PlayerPrefs.GetString(Key)?.ToDeserialized<PlayerProgress>();

        public void SaveProgress() =>
            PlayerPrefs.SetString(Key, _progressService.Progress.ToJson());
    }
}