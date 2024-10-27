using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers
{
    public class SelectFrame : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Vector3 _offset;

        private TileRepresentation _lastSelectedTile;

        private void Start() =>
            Hide();

        public void Select(TileRepresentation tile) =>
            transform.position = tile.BuildingPoint.position + _offset;

        public void SelectLast()
        {
            if (_lastSelectedTile != null)
                Select(_lastSelectedTile);
        }

        public void Show() =>
            _canvas.enabled = true;

        public void Hide() =>
            _canvas.enabled = false;

        public class Factory : PlaceholderFactory<string, UniTask<SelectFrame>>
        {
        }
    }
}
