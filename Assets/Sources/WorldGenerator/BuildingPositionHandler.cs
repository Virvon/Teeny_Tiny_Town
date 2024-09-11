using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.WorldGenerator
{
    public class BuildingPositionHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _buildingPrefab;

        public List<Tile> Tiles = new();

        private GameObject _building;

        public void Add(Tile tile)
        {
            tile.Soil.PointerMoved += OnPointerMoved;
        }

        private void OnPointerMoved(Soil soil)
        {
            if (_building == null)
                return;

            _building.transform.position = soil.BuildingPoint.position;
            _building.transform.parent = soil.transform;
        }
    }
}
