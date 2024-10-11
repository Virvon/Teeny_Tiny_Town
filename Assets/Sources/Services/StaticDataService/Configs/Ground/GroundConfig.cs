using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [Serializable]
    public class GroundConfig
    {
        public GroundType Type;
        public RoadConfig[] RoadConfigs;

        public RoadType GetRoadType(Vector2Int gridPosition, List<Vector2Int> adjacentGridPositions, out GroundRotation rotation)
        {
            rotation = default;

            Vector2Int[] normalizedNeighborsGridPositions = new Vector2Int[adjacentGridPositions.Count];

            for (int i = 0; i < normalizedNeighborsGridPositions.Length; i++)
                normalizedNeighborsGridPositions[i] = adjacentGridPositions[i] - gridPosition;

            foreach (RoadConfig roadConfig in RoadConfigs)
            {
                if (roadConfig.CheckSuitableNeighborsGridPositions(normalizedNeighborsGridPositions, out rotation))
                    return roadConfig.Type;
            }

            Debug.LogError("road type not founded");

            return default;
        }
    }
}
