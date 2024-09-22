using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure
{
    public class Ground
    {
        private readonly IStaticDataService _staticDataService;

        public Ground(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public GroundType Type { get; private set; }
        public RoadType RoadType { get; private set; }
        public GroundRotation Rotation { get; private set; }

        public bool TryChange(Vector2Int gridPosition, List<Vector2Int> adjacentGridPosition, GroundType targetGroundType)
        {
            RoadType newRoadType = _staticDataService.GroundsConfig.GetRoadType(gridPosition, adjacentGridPosition, targetGroundType, out GroundRotation rotation);

            if (RoadType == newRoadType && Type == targetGroundType && Rotation == rotation)
                return false;

            Type = targetGroundType;
            RoadType = newRoadType;
            Rotation = rotation;

            return true;
        }

        public void SetNonEmpty()
        {
            RoadType = RoadType.NonEmpty;
        }

        public void ChangeRoadType(RoadType type, GroundRotation rotation)
        {
            RoadType = type;
            Rotation = rotation;
        }

        public void ChangeGroundType(GroundType type)
        {
            Type = type;
        }
    }
}
