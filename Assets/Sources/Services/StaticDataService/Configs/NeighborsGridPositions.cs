using System;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [Serializable]
    public class NeighborsGridPositions
    {
        public GroundRotation Rotation;
        public Vector2Int[] GridPositions;
    }
}
