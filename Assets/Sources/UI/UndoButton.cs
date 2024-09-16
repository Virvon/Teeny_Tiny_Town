using Assets.Sources.Gameplay.WorldGenerator.Comand;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI
{
    public class UndoButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private GameplayMover _gameplayMover;

        [Inject]
        private void Construct(GameplayMover gameplayMover)
        {
            _gameplayMover = gameplayMover;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            _gameplayMover.TryUndoMove();
        }
    }
}
