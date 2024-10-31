using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI.Windows.Sandbox
{
    public class SandboxPanelsSwitcher : MonoBehaviour
    {
        [SerializeField] private SandboxPanel _buildingsPanel;
        [SerializeField] private SandboxPanel _tilesPanel;
        [SerializeField] private Button _removeButton;
        [SerializeField] private Button _buildingsButton;
        [SerializeField] private Button _tilesButton;

        private SandboxPanel _currentPanel;
        private bool _canSwitch;

        private void OnEnable()
        {
            _buildingsButton.onClick.AddListener(OnBuildingsButtonClicked);
            _tilesButton.onClick.AddListener(OnTilesButtonClicked);
            _removeButton.onClick.AddListener(OnRemoveButtonClicked);

            _canSwitch = true;
        }

        private void OnDisable()
        {
            _buildingsButton.onClick.RemoveListener(OnBuildingsButtonClicked);
            _tilesButton.onClick.RemoveListener(OnTilesButtonClicked);
            _removeButton.onClick.RemoveListener(OnRemoveButtonClicked);
        }

        private void OnRemoveButtonClicked()
        {
            if(_canSwitch)
                HideCurrentPanel();
        }

        private void OnTilesButtonClicked()
        {
            if(_currentPanel != _tilesPanel)
                SwitchPanels(_tilesPanel);
        }

        private void OnBuildingsButtonClicked()
        {
            if(_currentPanel != _buildingsPanel)
                SwitchPanels(_buildingsPanel);
        }

        private void SwitchPanels(SandboxPanel targetPanel)
        {
            if (_canSwitch == false)
                return;

            _canSwitch = false;

            HideCurrentPanel(callback: () =>
            {
                _currentPanel = targetPanel;
                _currentPanel.Open();
                _canSwitch = true;
            });
        }

        private void HideCurrentPanel(Action callback = null)
        {
            if (_currentPanel != null)
            {
                _currentPanel.Hide(callback: () =>
                {
                    _currentPanel = null;
                    callback?.Invoke();   
                });
            }
            else
                callback?.Invoke();
        }
    }
}
