using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "GroundsConfig", menuName = "StaticData/Create new grounds config", order = 51)]
    public class GroundsConfig : ScriptableObject
    {
        public GroundConfig[] GroundConfigs;

        public GroundType GetGroundType(Vector2Int gridPosition, List<Vector2Int> adjacentGridPositions, out GroundRotation rotation)
        {
            rotation = default;

            Vector2Int[] normalizedNeighborsGridPositions = new Vector2Int[adjacentGridPositions.Count];

            for(int i = 0; i < normalizedNeighborsGridPositions.Length; i++)
                normalizedNeighborsGridPositions[i] = adjacentGridPositions[i] - gridPosition;

            foreach (GroundConfig groundConfig in GroundConfigs)
            {
                if (groundConfig.CheckSuitableNeighborsGridPositions(normalizedNeighborsGridPositions, out rotation))
                    return groundConfig.Type;
            }

            Debug.LogError("type not founded");

            return default;
        }
    }
}
