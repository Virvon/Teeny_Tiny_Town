﻿using Assets.Sources.Gameplay.World;
using Assets.Sources.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.Start
{
    public class TutorialButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private StartWindowPanel _settingsPanel;

        private WorldsList _worldsList;
        private IPersistentProgressService _persistentProgressService;

        [Inject]
        private void Construct(WorldsList worldsList, IPersistentProgressService persistentProgressService)
        {
            _worldsList = worldsList;
            _persistentProgressService = persistentProgressService;

            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDestroy() =>
            _button.onClick.RemoveListener(OnButtonClicked);

        private async void OnButtonClicked()
        {
            _persistentProgressService.Progress.IsEducationCompleted = false;
            _settingsPanel.OpenNextPanel();
            await _worldsList.ChangeToEducationWorld(callback: _worldsList.StartCurrentWorld);
        }
    }
}