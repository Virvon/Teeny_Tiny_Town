using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.World.Panels
{
    public class RotationPanel : MonoBehaviour
    {
        [SerializeField] private Button _rotateWorldСlockwiseButton;
        [SerializeField] private Button _rotateWorldСounterclockwiseButton;

        private Gameplay.World.World _world;

        [Inject]
        private void Construct(Gameplay.World.World world)
        {
            _world = world;

            _rotateWorldСlockwiseButton.onClick.AddListener(OnRotateWorldClockwiseButtonClicked);
            _rotateWorldСounterclockwiseButton.onClick.AddListener(OnRotateWorldCounterclockwiseButtonClicked);
        }

        private void OnDestroy()
        {
            _rotateWorldСlockwiseButton.onClick.RemoveListener(OnRotateWorldClockwiseButtonClicked);
            _rotateWorldСounterclockwiseButton.onClick.RemoveListener(OnRotateWorldCounterclockwiseButtonClicked);
        }

        private void OnRotateWorldCounterclockwiseButtonClicked() =>
            _world.RotateСounterclockwise();

        private void OnRotateWorldClockwiseButtonClicked() =>
            _world.RotateСlockwise();

        public class Factory : PlaceholderFactory<string, Transform, UniTask<RotationPanel>>
        {
        }
    }
}
