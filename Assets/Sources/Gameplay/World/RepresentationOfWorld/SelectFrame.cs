using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld
{
    public class SelectFrame : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Vector3 _offset;

        private void Start() =>
            Hide();

        public void Select(Tile tile)
        {
            transform.position = tile.BuildingPoint.position + _offset;
            _canvas.enabled = true;
        }

        public void Hide() =>
            _canvas.enabled = false;

        public class Factory : PlaceholderFactory<string, UniTask<SelectFrame>>
        {
        }
    }
}
