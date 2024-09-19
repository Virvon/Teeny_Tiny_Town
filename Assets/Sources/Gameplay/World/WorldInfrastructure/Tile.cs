using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure
{
    public class Tile
    {
        public readonly Vector2Int GridPosition;

        private readonly IStaticDataService _staticDataService;
        private readonly Ground _ground;

        private List<Tile> _adjacentTiles;

        public Tile(Vector2Int greedPosition, IStaticDataService staticDataService)
        {
            GridPosition = greedPosition;
            _staticDataService = staticDataService;

            _adjacentTiles = new();
            _ground = new(staticDataService);
        }

        public BuildingType BuildingType { get; private set; }
        public bool IsEmpty => BuildingType == BuildingType.Undefined;
        public GroundType GroundType => _ground.Type;
        public GroundRotation GroundRotation => _ground.Rotation;

        public void AddAdjacentTile(Tile adjacentTile)
        {
            _adjacentTiles.Add(adjacentTile);
            ChangeGround();
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

        public void ChangeGroundChain(List<Tile> countedTiles)
        {
            countedTiles.Add(this);

            foreach(Tile tile in _adjacentTiles)
            {
                //if()
            }
        }

        private void ChangeGround()
        {
            List<Vector2Int> adjacentEmptyTileGridPositions = (_adjacentTiles.Where(tile => tile.IsEmpty).Select(tile => tile.GridPosition)).ToList();
            _ground.Change(GridPosition, adjacentEmptyTileGridPositions);
        }
    }
}
