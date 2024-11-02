using Assets.Sources.Services.PersistentProgress;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.Start
{
    public class OrthographicSettingsOption : MonoBehaviour
    {
        [SerializeField] private Toggle _toggle;

        private IPersistentProgressService _persistentProgressService;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService)
        {
            _persistentProgressService = persistentProgressService;

            _toggle.isOn = _persistentProgressService.Progress.SettingsData.IsOrthographicCamera;

            _toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        private void OnDestroy() =>
            _toggle.onValueChanged.RemoveListener(OnToggleValueChanged);

        private void OnToggleValueChanged(bool value) =>
            _persistentProgressService.Progress.SettingsData.ChangeOrthographic(value);
    }
}
