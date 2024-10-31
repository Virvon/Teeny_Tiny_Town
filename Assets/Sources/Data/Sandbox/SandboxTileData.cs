using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.World;
using System;
using UnityEngine;

namespace Assets.Sources.Data.Sandbox
{
    [Serializable]
    public class SandboxTileData : TileData
    {
        public TileType Type;
        public GroundType GroundType;

        public SandboxTileData(Vector2Int gridPosition)
            : base(gridPosition, BuildingType.Undefined)
        {
            Type = TileType.RoadGround;
            GroundType = GroundType.Soil;
        }
    }
}
