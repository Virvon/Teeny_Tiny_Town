using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Services.StaticDataService.Configs.AdditionalBonuses;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.Quests;
using Assets.Sources.Services.StaticDataService.Configs.Reward;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem.Utilities;

namespace Assets.Sources.Services.StaticDataService
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IAssetProvider _assetsProvider;

        private Dictionary<BuildingType, BuildingConfig> _buildingConfigs;
        private Dictionary<GroundType, Dictionary<RoadType, RoadConfig>> _groundConfigs;
        private Dictionary<WindowType, WindowConfig> _windowConfigs;
        private Dictionary<BuildingType, GameplayStoreItemConfig> _worldStoreItemConfigs;
        private Dictionary<TileType, TestGroundConfig> _testGroundConfigs;
        private Dictionary<BuildingType, GroundType> _roadGrounds;
        private Dictionary<string, WorldConfig> _worldConfigs;
        private Dictionary<RewardType, RewardConfig> _rewardConfigs;
        private Dictionary<GameplayStoreItemType, StoreItemConfig> _gameplayStoreItemConfigs;
        private Dictionary<AdditionalBonusType, AdditionalBonusConfig> _additionalBonusConfigs;

        public StaticDataService(IAssetProvider assetsProvider) =>
            _assetsProvider = assetsProvider;

        public GroundsConfig GroundsConfig { get; private set; }
        public GameplayStoreItemsConfig StoreItemsConfig { get; private set; }
        public WindowsConfig WindowsConfig { get; private set; }
        public WorldsConfig WorldsConfig { get; private set; }
        public AvailableForConstructionBuildingsConfig AvailableForConstructionBuildingsConfig { get; private set; }
        public ReadOnlyArray<WorldConfig> WorldConfigs => _worldConfigs.Values.ToArray();
        public AnimationsConfig AnimationsConfig { get; private set; }
        public QuestsConfig QuestsConfig { get; private set; }

        public async UniTask InitializeAsync()
        {
            List<UniTask> tasks = new ();

            tasks.Add(LoadBuildingConfigs());
            tasks.Add(LoadGroundsConfig());
            tasks.Add(LoadWindowsConfig());
            tasks.Add(LoadWorldStoreItemConfigs());
            tasks.Add(LoadWorldsConfig());
            tasks.Add(LoadRoadGroundConfigs());
            tasks.Add(LoadAvailableForConstructionBuildingsConfig());
            tasks.Add(LoadWorldConfigs());
            tasks.Add(LoadAnimationsConfig());
            tasks.Add(LoadGameplayStoreItemConfigs());
            tasks.Add(LoadRewardConfigs());
            tasks.Add(LoadQuestsConfig());
            tasks.Add(LoadAdditionalBonusConfigs());

            await UniTask.WhenAll(tasks);
        }

        public AdditionalBonusConfig GetAdditionalBonus(AdditionalBonusType type) =>
            _additionalBonusConfigs.TryGetValue(type, out AdditionalBonusConfig config) ? config : null;

        public StoreItemConfig GetGameplayStorItem(GameplayStoreItemType type) =>
            _gameplayStoreItemConfigs.TryGetValue(type, out StoreItemConfig config) ? config : null;

        public RewardConfig GetReward(RewardType type) =>
            _rewardConfigs.TryGetValue(type, out RewardConfig config) ? config : null;

        public TWorldConfig GetWorld<TWorldConfig>(string id)
            where TWorldConfig : WorldConfig =>
            _worldConfigs.TryGetValue(id, out WorldConfig config) ? config as TWorldConfig : null;

        public GroundType GetGroundType(BuildingType buildingType) =>
            _roadGrounds.TryGetValue(buildingType, out GroundType type) ? type : default;

        public TestGroundConfig GetGround(TileType tileType) =>
            _testGroundConfigs.TryGetValue(tileType, out TestGroundConfig config) ? config : null;

        public GameplayStoreItemConfig GetWorldStoreItem(BuildingType buildingType) =>
            _worldStoreItemConfigs.TryGetValue(buildingType, out GameplayStoreItemConfig config) ? config : null;

        public WindowConfig GetWindow(WindowType type) =>
            _windowConfigs.TryGetValue(type, out WindowConfig config) ? config : null;

        public TBuilding GetBuilding<TBuilding>(BuildingType buildingType)
            where TBuilding : BuildingConfig =>
            _buildingConfigs.TryGetValue(buildingType, out BuildingConfig config) ? config as TBuilding : null;

        public RoadConfig GetRoad(GroundType groundType, RoadType roadType)
        {
            return _groundConfigs.TryGetValue(
                groundType, out Dictionary<RoadType, RoadConfig> roadConfigs) ? (roadConfigs.TryGetValue(
                roadType, out RoadConfig config) ? config : null) : null;
        }

        private async UniTask LoadAdditionalBonusConfigs()
        {
            AdditionalBonusConfig[] configs = await GetConfigs<AdditionalBonusConfig>();

            _additionalBonusConfigs = configs.ToDictionary(value => value.Type, value => value);
        }


        private async UniTask LoadQuestsConfig()
        {
            QuestsConfig[] configs = await GetConfigs<QuestsConfig>();

            QuestsConfig = configs.First();
        }

        private async UniTask LoadRewardConfigs()
        {
            RewardConfig[] configs = await GetConfigs<RewardConfig>();

            _rewardConfigs = configs.ToDictionary(value => value.Type, value => value);
        }

        private async UniTask LoadGameplayStoreItemConfigs()
        {
            StoreItemConfig[] configs = await GetConfigs<StoreItemConfig>();

            _gameplayStoreItemConfigs = configs.ToDictionary(value => value.Type, value => value);
        }

        private async UniTask LoadAnimationsConfig()
        {
            AnimationsConfig[] configs = await GetConfigs<AnimationsConfig>();

            AnimationsConfig = configs.First();
        }

        private async UniTask LoadWorldConfigs()
        {
            WorldConfig[] worldConfigs = await GetConfigs<WorldConfig>();

            _worldConfigs = worldConfigs.ToDictionary(value => value.Id, value => value);
        }

        private async UniTask LoadAvailableForConstructionBuildingsConfig()
        {
            AvailableForConstructionBuildingsConfig[] configs = await GetConfigs<AvailableForConstructionBuildingsConfig>();

            AvailableForConstructionBuildingsConfig = configs.First();
        }

        private async UniTask LoadRoadGroundConfigs()
        {
            RoadGroundConfigs[] roadGroundConfigs = await GetConfigs<RoadGroundConfigs>();

            _roadGrounds = roadGroundConfigs.First().Configs.ToDictionary(value => value.BuildingType, value => value.GroundType);
        }

        private async UniTask LoadWorldsConfig()
        {
            WorldsConfig[] worldsConfigs = await GetConfigs<WorldsConfig>();

            WorldsConfig = worldsConfigs.First();
        }

        private async UniTask LoadWorldStoreItemConfigs()
        {
            GameplayStoreItemsConfig[] storeItemsConfigs = await GetConfigs<GameplayStoreItemsConfig>();

            StoreItemsConfig = storeItemsConfigs.First();

            _worldStoreItemConfigs = StoreItemsConfig.Configs.ToDictionary(value => value.BuildingType, value => value);
        }

        private async UniTask LoadWindowsConfig()
        {
            WindowsConfig[] windowsConfigs = await GetConfigs<WindowsConfig>();
            WindowsConfig = windowsConfigs.First();

            _windowConfigs = WindowsConfig.Configs.ToDictionary(value => value.Type, value => value);
        }

        private async UniTask LoadGroundsConfig()
        {
            GroundsConfig[] groundsConfigs = await GetConfigs<GroundsConfig>();

            GroundsConfig = groundsConfigs.First();
            _groundConfigs = GroundsConfig.GroundConfigs.ToDictionary(value => value.Type, value => value.RoadConfigs.ToDictionary(value => value.Type, value => value));
            _testGroundConfigs = GroundsConfig.TestGroundConfigs.ToDictionary(value => value.TileType, value => value);
        }

        private async UniTask LoadBuildingConfigs()
        {
            BuildingConfig[] mergeConfigs = await GetConfigs<BuildingConfig>();

            _buildingConfigs = mergeConfigs.ToDictionary(value => value.BuildingType, value => value);
        }

        private async UniTask<TConfig[]> GetConfigs<TConfig>()
            where TConfig : class
        {
            List<string> keys = await GetConfigKeys<TConfig>();
            return await _assetsProvider.LoadAll<TConfig>(keys);
        }

        private async UniTask<List<string>> GetConfigKeys<TConfig>() =>
            await _assetsProvider.GetAssetsListByLabel<TConfig>(AssetLabels.Configs);
    }
}