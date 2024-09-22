using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [Serializable]
    public class RoadConfig
    {
        public RoadType Type;
        public AssetReferenceGameObject AssetReference;
        public ValidNeighborsGridPositions[] ValidNeighborsGridPositions;

        public bool CheckSuitableNeighborsGridPositions(Vector2Int[] normalizedNeighborsGridPositions, out GroundRotation groundRotation)
        {
            groundRotation = default;

            foreach (ValidNeighborsGridPositions neighborGridPositions in ValidNeighborsGridPositions)
            {
                if (normalizedNeighborsGridPositions.Length == neighborGridPositions.NormalizedGridPositions.Length && neighborGridPositions.NormalizedGridPositions.Except(normalizedNeighborsGridPositions).Count() == 0)
                {
                    groundRotation = neighborGridPositions.Rotation;

                    return true;
                }
            }

            return false;
        }
    }
}
