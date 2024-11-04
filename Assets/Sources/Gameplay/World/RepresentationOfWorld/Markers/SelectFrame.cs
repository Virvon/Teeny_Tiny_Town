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

        private void Start() =>
            Hide("self start");

        public void Select(TileRepresentation tile)
        {
            if (tile == null)
            {
                Hide("self");

                return;
            }

            transform.position = tile.BuildingPoint.position + _offset;
        }

        public void Show() =>
            _canvas.enabled = true;

        public void Hide(string str)
        {
            Debug.Log(str);
            _canvas.enabled = false;
        }

        public class Factory : PlaceholderFactory<string, Transform, UniTask<SelectFrame>>
        {
        }
    }
}
