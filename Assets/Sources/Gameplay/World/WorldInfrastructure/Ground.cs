using Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure
{
    public class Ground
    {
        private readonly IStaticDataService _staticDataService;

        private GroundType _currentCreatedRoadType;

        public Ground(IStaticDataService staticDataService, GroundType type)
        {
            _staticDataService = staticDataService;
            Type = type;
            _currentCreatedRoadType = Type;
        }

        public GroundType Type { get; private set; }
        public RoadType RoadType { get; private set; }
        public GroundRotation Rotation { get; private set; }

        public bool TrySet(Vector2Int gridPosition, List<Vector2Int> adjacentGridPosition, GroundType targetGroundType)
        {
            RoadType newRoadType = _staticDataService.GroundsConfig.GetRoadType(gridPosition, adjacentGridPosition, targetGroundType, out GroundRotation rotation);

            if (RoadType == newRoadType && _currentCreatedRoadType == targetGroundType && Rotation == rotation)
                return false;

            Type = targetGroundType;
            RoadType = newRoadType;
            Rotation = rotation;
            _currentCreatedRoadType = Type;

            return true;
        }

        public bool TryTakeAroundTilesGroundType(List<RoadTile> aroundTiles, bool isEmpty)
        {
            if (isEmpty == false)
                return false;

            bool isChanged = false;
            GroundType newType = GroundType.Soil;

            foreach (RoadTile tile in aroundTiles)
            {
                if (tile.IsEmpty == false && (int)tile.Ground.Type > (int)newType)
                {
                    newType = tile.Ground.Type;
                    isChanged = true;
                }
            }

            Type = newType;

            return isChanged;
        }

        public void SetEmpty(List<RoadTile> aroundTiles)
        {
            Type = GroundType.Soil;

            TryTakeAroundTilesGroundType(aroundTiles, true);
        }

        public bool TryValidateRoad(List<RoadTile> adjacentTiles, bool isEmpty, Vector2Int gridPosition)
        {
            if (isEmpty)
            {
                List<Vector2Int> adjacentEmptyTileGridPositions = adjacentTiles
                    .Where(tile => tile.IsEmpty && tile.Ground.Type == Type)
                    .Select(tile => tile.GridPosition)
                    .ToList();

                return TrySet(gridPosition, adjacentEmptyTileGridPositions, Type);
            }
            else
            {
                RoadType = RoadType.NonEmpty;

                return true;
            }
        }

        public bool TryUpdate(BuildingType buildingType)
        {
            GroundType newType = _staticDataService.GetGroundType(buildingType);

            if (newType == Type)
                return false;

            Type = newType;

            return true;
        }

        public void SetNonEmpty()
        {
            RoadType = RoadType.NonEmpty;
        }

        public void ChangeGroundType(GroundType type)
        {
            Type = type;
        }
    }
}
