using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.UI.Windows.World;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Windows.Education
{
    public class EducationWindow : CurrencyGameplayWindow
    {
        [SerializeField] private EducationPanel[] _educationPanels;

        private IPersistentProgressService _persistentProgressService;

        private int _currentEducationPanelIndex;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService) =>
            _persistentProgressService = persistentProgressService;

        public override void Open()
        {
            base.Open();

            if (_persistentProgressService.Progress.IsEducationCompleted)
            {
                _educationPanels.Last().Hide();
            }
            else
            {
                _currentEducationPanelIndex = 0;
                _educationPanels[_currentEducationPanelIndex].Open();
            }
        }

        protected override void Subscrube()
        {
            base.Subscrube();

            foreach (EducationPanel educationPanel in _educationPanels)
                educationPanel.Handled += OnEducationHandled;
        }

        protected override void Unsubscruby()
        {
            base.Unsubscruby();

            foreach (EducationPanel educationPanel in _educationPanels)
                educationPanel.Handled -= OnEducationHandled;
        }

        private void OnEducationHandled()
        {
            _educationPanels[_currentEducationPanelIndex].Hide(callback: () =>
            {
                _currentEducationPanelIndex++;

                if (_currentEducationPanelIndex < _educationPanels.Length)
                    _educationPanels[_currentEducationPanelIndex].Open();
            });
        }
    }
}
