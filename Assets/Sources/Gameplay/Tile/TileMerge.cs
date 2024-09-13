using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Gameplay.Tile
{
    public class TileMerge : MonoBehaviour
    {
        private const int MinTilesCountToMerge = 3;

        private List<Tile> _adjacentTiles;

        public BuildingType BuildingType { get; private set; }

        public Vector2Int GridPosition { get; private set; }

        public void Init(List<Tile> adjacentTiles)
        {
            _adjacentTiles = adjacentTiles;
        }

        public void Init(Vector2Int gridPosition)
        {
            GridPosition = gridPosition;
        }

        public void SetBuilding(Building building)
        {
            BuildingType = building.Type;

            Debug.Log("count " + CheckMergeValidity(new List<TileMerge>()));
        }

        public int CheckMergeValidity(List<TileMerge> countedTiles)
        {
            int tilesCountInChain = 1;
            countedTiles.Add(this);

            foreach(Tile tile in _adjacentTiles)
            {
                if(tile.TileMerge.BuildingType == BuildingType && countedTiles.Contains(tile.TileMerge) == false)
                {
                    tilesCountInChain += tile.TileMerge.CheckMergeValidity(countedTiles);
                }
            }

            return tilesCountInChain;
        }
    }
}
