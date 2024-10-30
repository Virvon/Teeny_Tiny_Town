using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.World
{
    [Serializable]
    public class ExpandConfig
    {
        public BuildingType BuidldingType;
        public Vector2Int ExpandedSize;
    }
}
