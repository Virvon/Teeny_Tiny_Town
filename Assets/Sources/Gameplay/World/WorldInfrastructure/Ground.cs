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
        public GroundRotation Rotation { get; private set; }

        public bool TryChange(Vector2Int gridPosition, List<Vector2Int> adjacentGridPosition)
        {
            GroundType newGroundType = _staticDataService.GroundsConfig.GetGroundType(gridPosition, adjacentGridPosition, out GroundRotation newGroundRotation);

            if (Type == newGroundType && Rotation == newGroundRotation)
                return false;

            Type = newGroundType;
            Rotation = newGroundRotation;

            return true;
        }

        public void SetSoil()
        {
            Type = GroundType.Soil;
        }

        public void Change(GroundType type, GroundRotation rotation)
        {
            Type = type;
            Rotation = rotation;
        }
    }
}
