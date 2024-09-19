using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure
{
    public class Ground
    {
        private readonly IStaticDataService _staticDataService;

        private GroundRotation _rotation;

        public Ground(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public GroundType Type { get; private set; }
        public GroundRotation Rotation => _rotation;

        public void Change(Vector2Int gridPosition, List<Vector2Int> adjacentGridPosition)
        {
            Type = _staticDataService.GroundsConfig.GetGroundType(gridPosition, adjacentGridPosition, out _rotation);
        }

        public void Change()
        {
            Type = GroundType.Soil;
        }
    }
}
