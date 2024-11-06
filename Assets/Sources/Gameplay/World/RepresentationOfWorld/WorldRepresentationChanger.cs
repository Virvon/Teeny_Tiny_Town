using System;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Gameplay.World.WorldInfrastructure.NextBuildingForPlacing;
using Assets.Sources.Services.SaveLoadProgress;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld
{
    public class WorldRepresentationChanger
    {
        private readonly IWorldChanger _worldChanger;
        private readonly IWorldFactory _worldFactory;
        private readonly NextBuildingForPlacingCreator _nextBuildingForPlacingCreator;
        private readonly ISaveLoadService _saveLoadService;

        public WorldRepresentationChanger(
            IWorldChanger worldChanger,
            IWorldFactory worldFactory,
            NextBuildingForPlacingCreator nextBuildingForPlacingCreator,
            ISaveLoadService saveLoadService)
        {
            _worldChanger = worldChanger;

            _worldChanger.TilesChanged += OnTilesChanged;

            _worldFactory = worldFactory;
            _nextBuildingForPlacingCreator = nextBuildingForPlacingCreator;
            _saveLoadService = saveLoadService;
        }

        ~WorldRepresentationChanger()
        {
            _worldChanger.TilesChanged -= OnTilesChanged;
        }

        public event Action GameplayMoved;

        public TileRepresentation StartTile => WorldGenerator.GetTile(_nextBuildingForPlacingCreator.BuildingsForPlacingData.StartGridPosition);

        private WorldGenerator WorldGenerator => _worldFactory.WorldGenerator;

        private void OnTilesChanged()
        {
            GameplayMoved?.Invoke();
            _saveLoadService.SaveProgress();
        }
    }
}
