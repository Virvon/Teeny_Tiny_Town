using System;
using Assets.Sources.Data.World;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.Quests;

namespace Assets.Sources.Gameplay
{
    public class QuestsChecker : IDisposable
    {
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly IWorldData _worldData;
        private readonly IWorldChanger _worldChanger;
        private readonly IStaticDataService _staticDataService;

        public QuestsChecker(
            IPersistentProgressService persistentProgressService,
            IWorldData worldData,
            IWorldChanger worldChanger,
            IStaticDataService staticDataService)
        {
            _persistentProgressService = persistentProgressService;
            _worldData = worldData;
            _worldChanger = worldChanger;
            _staticDataService = staticDataService;

            _worldData.BuildingUpgraded += OnBuildingPlaced;
            _worldChanger.BuildingPlaced += OnBuildingPlaced;
        }

        public void Dispose()
        {
            _worldData.BuildingUpgraded -= OnBuildingPlaced;
            _worldChanger.BuildingPlaced -= OnBuildingPlaced;
        }

        private void OnBuildingPlaced(BuildingType type)
        {
            foreach (Data.QuestData questData in _persistentProgressService.Progress.Quests)
            {
                QuestConfig questConfig = _staticDataService.QuestsConfig.GetQuest(questData.Id);

                if (type == questConfig.BuildingType)
                    questData.Perform(questConfig.TargetCount);
            }
        }
    }
}
