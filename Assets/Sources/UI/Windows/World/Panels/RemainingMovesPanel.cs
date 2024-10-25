using Assets.Sources.Services.PersistentProgress;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Windows.World.Panels
{
    public class RemainingMovesPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _remainingMovesCountValue;

        private IPersistentProgressService _persistentProgressService;

        [Inject]
        public void Construct(IPersistentProgressService persistentProgressService)
        {
            _persistentProgressService = persistentProgressService;

            _persistentProgressService.Progress.GameplayMovesCounter.RemainingMovesCountChanged += OnRemainingMovesCountChanged;

            OnRemainingMovesCountChanged();
        }

        private void OnDestroy() =>
            _persistentProgressService.Progress.GameplayMovesCounter.RemainingMovesCountChanged -= OnRemainingMovesCountChanged;

        private void OnRemainingMovesCountChanged() =>
            _remainingMovesCountValue.text = _persistentProgressService.Progress.GameplayMovesCounter.RemainingMovesCount.ToString();

        public class Factory : PlaceholderFactory<string, Transform, UniTask<RemainingMovesPanel>>
        {
        }
    }
}
