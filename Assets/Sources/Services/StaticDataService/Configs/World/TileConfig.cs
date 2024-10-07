using Assets.Sources.Gameplay.World.WorldInfrastructure;
using System;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.World
{
    [Serializable]
    public class TileConfig
    {
        public TileType Type;
        public Vector2Int GridPosition;
        public BuildingType BuildingType;

        public TileConfig(Vector2Int gridPosition)
        {
            GridPosition = gridPosition;
        }
    }
}
