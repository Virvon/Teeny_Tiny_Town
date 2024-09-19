using Assets.Sources.Services.StaticDataService;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure
{
    public class Tile
    {
        public readonly Vector2Int GridPosition;

        private readonly IStaticDataService _staticDataService;

        private List<Tile> _adjacentTiles;

        public Tile(Vector2Int greedPosition, IStaticDataService staticDataService)
        {
            GridPosition = greedPosition;
            _staticDataService = staticDataService;

            _adjacentTiles = new();
        }

        public BuildingType BuildingType { get; private set; }

        public void AddAdjacentTile(Tile adjacentTile)
        {
            _adjacentTiles.Add(adjacentTile);
        }

        public void PutBuilding(BuildingType buildingType)
        {
            BuildingType = buildingType;
        }

        public int GetTilesChainCount(List<Tile> countedTiles)
        {
            int tilesCountInChain = 1;
            countedTiles.Add(this);

            foreach (Tile tile in _adjacentTiles)
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
