﻿using Assets.Sources.Gameplay.World.WorldInfrastructure;
using UnityEngine;

namespace Assets.Sources.Data
{
    public class TileData
    {
        public Vector2Int GridPosition;
        public BuildingType BuildingType;

        public TileData(Vector2Int gridPosition, BuildingType buildingType)
        {
            GridPosition = gridPosition;
            BuildingType = buildingType;
        }
    }
}
