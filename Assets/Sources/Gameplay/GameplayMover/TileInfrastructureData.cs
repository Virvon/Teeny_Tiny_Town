﻿using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Services.StaticDataService.Configs;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public class TileInfrastructureData
    {
        public readonly Vector2Int GridPosition;
        public readonly BuildingType BuildingType;
        public readonly RoadType GroundType;
        public readonly GroundRotation GroundRotation;

        public TileInfrastructureData(Vector2Int tileGridPosition, BuildingType buildingType, RoadType groundType, GroundRotation groundRotation)
        {
            GridPosition = tileGridPosition;
            BuildingType = buildingType;
            GroundType = groundType;
            GroundRotation = groundRotation;
        }
    }
}