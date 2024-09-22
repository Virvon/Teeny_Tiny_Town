using System;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [Serializable]
    public class ValidNeighborsGridPositions
    {
        public GroundRotation Rotation;
        public Vector2Int[] NormalizedGridPositions;
    }
}
