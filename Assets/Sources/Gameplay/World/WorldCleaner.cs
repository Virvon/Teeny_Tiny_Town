using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.World;
using System;
using System.Linq;

namespace Assets.Sources.Gameplay.World
{
    public class WorldCleaner : IDisposable
    {
        private readonly World _world;
        private readonly IWorldData _worldData;
        private readonly IStaticDataService _staticDataService;
        private readonly IWorldChanger _worldChanger;

        public WorldCleaner(World world, IWorldData worldData, IStaticDataService staticDataService, IWorldChanger worldChanger)
        {
            _world = world;

            _world.Cleaned += OnWorldCleaned;
            _worldData = worldData;
            _staticDataService = staticDataService;
            _worldChanger = worldChanger;
        }

        public void Dispose() =>
            _world.Cleaned -= OnWorldCleaned;

        private void OnWorldCleaned()
        {
            WorldConfig worldConfig = _staticDataService.GetWorld<WorldConfig>(_worldData.Id);

            _worldData.IsChangingStarted = false;
            _worldData.Update(worldConfig.TilesDatas, worldConfig.NextBuildingTypeForCreation, worldConfig.StartingAvailableBuildingTypes.ToList());
            _worldChanger.Update(true);
        }
    }
}
