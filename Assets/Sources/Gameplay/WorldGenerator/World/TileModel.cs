using Assets.Sources.Gameplay.Tile;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Gameplay.WorldGenerator.World
{
    public class TileModel
    {
        public readonly Vector2Int GridPosition;

        private List<TileModel> _adjacentTiles;
        private BuildingType _buildingType;

        public TileModel(Vector2Int greedPosition)
        {
            GridPosition = greedPosition;

            _adjacentTiles = new();
        }

        public void AddAdjacentTile(TileModel adjacentTile)
        {
            _adjacentTiles.Add(adjacentTile);
        }

        public void ChangeBuilding(BuildingType buildingType)
        {
            _buildingType = buildingType;
        }

        public int GetTilesChainCount(List<TileModel> countedTiles)
        {
            int tilesCountInChain = 1;
            countedTiles.Add(this);

            foreach (TileModel tile in _adjacentTiles)
            {
                if (_buildingType == tile._buildingType && countedTiles.Contains(tile) == false)
                    tilesCountInChain += tile.GetTilesChainCount(countedTiles);
            }

            return tilesCountInChain;
        }
    }
}
