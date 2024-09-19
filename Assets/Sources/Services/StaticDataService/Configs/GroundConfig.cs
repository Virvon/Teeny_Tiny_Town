using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [Serializable]
    public class GroundConfig
    {
        public GroundType Type;
        public AssetReferenceGameObject AssetReference;
        public NeighborsGridPositions[] ValidNeighborsGridPositions;

        public bool CheckSuitableNeighborsGridPositions(Vector2Int[] normalizedNeighborsGridPositions, out GroundRotation groundRotation)
        {
            groundRotation = default;

            foreach(NeighborsGridPositions neighborGridPositions in ValidNeighborsGridPositions)
            {
                if (normalizedNeighborsGridPositions.Length == neighborGridPositions.GridPositions.Length && neighborGridPositions.GridPositions.Except(normalizedNeighborsGridPositions).Count() == 0)
                {
                    groundRotation = neighborGridPositions.Rotation;

                    return true;
                }
            }

            return false;
        }
    }
}
