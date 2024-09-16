using Assets.Sources.Gameplay.Tile;
using Assets.Sources.Services.StaticDataService;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Gameplay.WorldGenerator.World
{
    public class TileModel
    {
        public readonly Vector2Int GridPosition;

        private readonly IStaticDataService _staticDataService;

        private List<TileModel> _adjacentTiles;

        public TileModel(Vector2Int greedPosition, IStaticDataService staticDataService)
        {
            GridPosition = greedPosition;
            _staticDataService = staticDataService;

            _adjacentTiles = new();
        }

        public BuildingType BuildingType { get; private set; }

        public void AddAdjacentTile(TileModel adjacentTile)
        {
            _adjacentTiles.Add(adjacentTile);
        }

        public void PutBuilding(BuildingType buildingType)
        {
            BuildingType = buildingType;
        }

        public int GetTilesChainCount(List<TileModel> countedTiles)
        {
            int tilesCountInChain = 1;
            countedTiles.Add(this);

            foreach (TileModel tile in _adjacentTiles)
            {
                if (BuildingType == tile.BuildingType && countedTiles.Contains(tile) == false)
                    tilesCountInChain += tile.GetTilesChainCount(countedTiles);
            }

            return tilesCountInChain;
        }

        public void Clean()
        {
            BuildingType = BuildingType.Undefined;
        }

        public void UpdateBuilding()
        {
            BuildingType = _staticDataService.GetMerge(BuildingType).NextBuilding;
        }
    }
}
