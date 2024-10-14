using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;
using UnityEngine;

namespace Assets.Sources.Data
{
    [Serializable]
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
