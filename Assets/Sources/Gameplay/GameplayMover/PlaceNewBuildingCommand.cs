using Assets.Sources.Data;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Services.StaticDataService;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public class PlaceNewBuildingCommand : Command
    {
        private readonly Vector2Int _placedBuildingGridPosition;
        private readonly BuildingType _placedBuildingType;
        private readonly WorldData _worldData;
        private readonly IStaticDataService _staticDataService;

        private readonly BuildingType _nextBuildingTypeForCreation;
        private readonly uint _nextBuildingForCreationBuildsCount;
        private readonly List<BuildingType> _availableBuildingForCreation;

        public PlaceNewBuildingCommand(WorldChanger world, Vector2Int placedBuildingGridPosition, WorldData worldData, IStaticDataService staticDataService)
            : base(world)
        {
            _placedBuildingGridPosition = placedBuildingGridPosition;
            _placedBuildingType = world.BuildingForPlacing.Type;
            _worldData = worldData;
            _staticDataService = staticDataService;

            _nextBuildingTypeForCreation = _worldData.NextBuildingTypeForCreation;
            _nextBuildingForCreationBuildsCount = _worldData.NextBuildingForCreationBuildsCount;
            _availableBuildingForCreation = _worldData.AvailableBuildingForCreation;
        }

        public override async void Change() =>
            await WorldChanger.PlaceNewBuilding(_placedBuildingGridPosition, _placedBuildingType);

        public override UniTask Undo()
        {
            _worldData.NextBuildingTypeForCreation = _nextBuildingTypeForCreation;
            _worldData.NextBuildingForCreationBuildsCount = _nextBuildingForCreationBuildsCount;
            _worldData.AvailableBuildingForCreation = _availableBuildingForCreation;

            return base.Undo();
        }
    }
}
