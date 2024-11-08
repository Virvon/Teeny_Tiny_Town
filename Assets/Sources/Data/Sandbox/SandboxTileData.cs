using System;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.World;
using UnityEngine;

namespace Assets.Sources.Data.Sandbox
{
    [Serializable]
    public class SandboxTileData : TileData
    {
        public SandboxGroundType GroundType;

        public SandboxTileData(Vector2Int gridPosition)
            : base(gridPosition, BuildingType.Undefined)
        {
            GroundType = SandboxGroundType.Soil;
        }
    }
}
