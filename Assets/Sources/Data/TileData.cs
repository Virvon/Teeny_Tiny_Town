using UnityEngine;

namespace Assets.Sources.Data
{
    public class TileData
    {
        public Vector2Int GridPosition;

        public TileData(Vector2Int gridPosition)
        {
            GridPosition = gridPosition;
        }
    }
}
