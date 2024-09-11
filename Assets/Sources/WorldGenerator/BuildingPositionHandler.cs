using Assets.Sources.Services.Input;
using UnityEngine;
using Zenject;

namespace Assets.Sources.WorldGenerator
{
    public class BuildingPositionHandler : MonoBehaviour
    {
        [SerializeField] private SelectFrame _selectFramePrefab;
        [SerializeField] private Vector3 _selectFramePositionOffset;
        [SerializeField] private float _raycastDistance;
        [SerializeField] private LayerMask _layerMask;

        private IInputService _inputService;
        private SelectFrame _selectFrame;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;

            _selectFrame = Instantiate(_selectFramePrefab);

            _inputService.HandleMoved += OnHandleMoved;
        }

        private void OnHandleMoved(Vector2 handlePosition)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(handlePosition.x, handlePosition.y, 1));

            if(Physics.Raycast(ray, out RaycastHit hitInfo, _raycastDistance, _layerMask, QueryTriggerInteraction.Ignore) && hitInfo.transform.TryGetComponent(out Soil soil))
            {
                _selectFrame.transform.position = soil.BuildingPoint.position + _selectFramePositionOffset;
                _selectFrame.gameObject.SetActive(true);
            }
        }
    }
}
