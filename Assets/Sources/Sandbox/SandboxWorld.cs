using Assets.Sources.Services.Input;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Sandbox
{
    public class SandboxWorld : MonoBehaviour
    {
        [SerializeField] private float _rotationSensivity;

        private IInputService _inputService;

        public float Rotation { get; private set; }

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;

            Rotation = transform.rotation.y;

            _inputService.Rotated += OnRotated;
        }

        private void OnDestroy() =>
            _inputService.Rotated -= OnRotated;

        private void OnRotated(float value)
        {
            Rotation += value;
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, Rotation, transform.rotation.z));
        }

        public class Factory : PlaceholderFactory<string, UniTask<SandboxWorld>>
        {
        }
    }
}
