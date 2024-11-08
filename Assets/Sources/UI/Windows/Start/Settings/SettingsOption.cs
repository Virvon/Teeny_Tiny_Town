using Assets.Sources.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.Start
{
    public abstract class SettingsOption : MonoBehaviour
    {
        [SerializeField] private Toggle _toggle;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService)
        {
            PersistentProgressService = persistentProgressService;

            _toggle.isOn = SetUpToggle();

            _toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        protected Toggle Toggle => _toggle;

        protected IPersistentProgressService PersistentProgressService { get; private set; }

        private void OnDestroy() =>
            _toggle.onValueChanged.RemoveListener(OnToggleValueChanged);

        protected abstract void OnToggleValueChanged(bool value);

        protected abstract bool SetUpToggle();
    }
}
