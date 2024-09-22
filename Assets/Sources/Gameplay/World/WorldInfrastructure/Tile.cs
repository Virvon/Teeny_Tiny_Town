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

        private List<Tile> _adjacentTiles;
        private List<Tile> _aroundTiles;

        public Tile(Vector2Int greedPosition, IStaticDataService staticDataService)
        {
            GridPosition = greedPosition;
            _staticDataService = staticDataService;

            _adjacentTiles = new();
            _aroundTiles = new();
            Ground = new(staticDataService);
        }

        public Ground Ground { get; private set; }
        public BuildingType BuildingType { get; private set; }
        public bool IsEmpty => BuildingType == BuildingType.Undefined;

        public void AddAdjacentTile(Tile adjacentTile)
        {
            _adjacentTiles.Add(adjacentTile);
        }

        public void AddAroundTile(Tile aroundTile)
        {
            _aroundTiles.Add(aroundTile);
        }

        public void PutBuilding(BuildingType buildingType)
        {
            BuildingType = buildingType;
            ChangeGroundType();
        }

        public void PutGround(RoadType type, GroundRotation rotation)
        {
            Ground.ChangeRoadType(type, rotation);
        }

        public void UpdateBuilding()
        {
            BuildingType = _staticDataService.GetBuilding(BuildingType).NextBuilding;
            ChangeGroundType();
        }

        public void RemoveBuilding()
        {
            BuildingType = BuildingType.Undefined;
            ChangeGroundType();
        }

        public int GetBuildingsChainLength(List<Tile> countedTiles)
        {
            int chainLength = 1;
            countedTiles.Add(this);

            foreach (Tile tile in _adjacentTiles)
            {
                if (BuildingType == tile.BuildingType && countedTiles.Contains(tile) == false)
                    chainLength += tile.GetBuildingsChainLength(countedTiles);
            }

            return chainLength;
        }

        public List<Tile> ChangeRoadsInChain(List<Tile> countedTiles, List<Tile> changedTiles)
        {
            countedTiles.Add(this);
            
            if(TryChangeRoad())
                changedTiles.Add(this);

            foreach (Tile tile in _adjacentTiles)
            {
                if (tile.IsEmpty && countedTiles.Contains(tile) == false)
                    tile.ChangeRoadsInChain(countedTiles, changedTiles);
            }

            return changedTiles;
        }

        public List<Tile> ChangeGroundsInChain(List<Tile> countedTiles, List<Tile> changedTiles)
        {
            countedTiles.Add(this);

            if (TryChangeGroundType())
                changedTiles.Add(this);

            foreach(Tile tile in _aroundTiles)
            {
                if(tile.IsEmpty && countedTiles.Contains(tile) == false)
                    tile.ChangeGroundsInChain(countedTiles, changedTiles);
            }

            return changedTiles;
        }

        private bool TryChangeGroundType()
        {
            if (IsEmpty == false)
                return false;

            GroundType groundType = GroundType.Soil;

            foreach (Tile tile in _aroundTiles)
            {
                if (tile.IsEmpty == false && (int)tile.Ground.Type > (int)groundType)
                    groundType = tile.Ground.Type;
            }

            bool isChanged = groundType != Ground.Type;

            Ground.ChangeGroundType(groundType);

            return isChanged;
        }

        private void ChangeGroundType() =>
            Ground.ChangeGroundType(IsEmpty ? GroundType.Soil : _staticDataService.GetBuilding(BuildingType).GroundType);

        private bool TryChangeRoad()
        {
            if (IsEmpty)
            {
                List<Vector2Int> adjacentEmptyTileGridPositions = (_adjacentTiles.Where(tile => tile.IsEmpty && tile.Ground.Type == Ground.Type).Select(tile => tile.GridPosition)).ToList();

                return Ground.TryChange(GridPosition, adjacentEmptyTileGridPositions, Ground.Type);
            }
            else
            {
                Ground.SetNonEmpty();

                return true;
            }    
        }
            
    }
}
