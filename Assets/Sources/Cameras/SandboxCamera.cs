using Assets.Sources.Services.Input;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Cameras
{
    public class SandboxCamera : MonoBehaviour
    {
        [SerializeField] private float _maxZoom;
        [SerializeField] private float _zoomSensivity;

        private IInputService _inputService;

        private Vector3 _minZoomPosition;
        private Vector3 _maxZoomPosition;
        private float _zoom;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;

            _zoom = 0.5f;

            _inputService.Zoomed += OnZoomed;
        }

        private void Start()
        {
            _maxZoomPosition = transform.position + transform.forward * _maxZoom / 2;
            _minZoomPosition = transform.position - transform.forward * _maxZoom / 2;
        }

        private void OnDestroy() =>
            _inputService.Zoomed -= OnZoomed;

        private void OnZoomed(float value)
        {
            _zoom += value * _zoomSensivity;
            _zoom = Mathf.Clamp(_zoom, 0, 1);
            transform.position = Vector3.Lerp(_minZoomPosition, _maxZoomPosition, _zoom);
        }
    }
}
