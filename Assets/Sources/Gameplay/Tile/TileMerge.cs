using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Gameplay.Tile
{
    public class TileMerge : MonoBehaviour
    {
        private List<Tile> _adjacentTiles;

        public Vector2Int GridPosition { get; private set; }

        public void Init(List<Tile> adjacentTiles)
        {
            _adjacentTiles = adjacentTiles;
        }

        public void Init(Vector2Int gridPosition)
        {
            GridPosition = gridPosition;
        }
    }
}
