﻿using System.Collections.Generic;
using System.Linq;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Services.StaticDataService
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IAssetProvider _assetsProvider;

        private Dictionary<BuildingType, BuildingConfig> _buildingConfigs;
        private Dictionary<GroundType, Dictionary<RoadType, RoadConfig>> _groundConfigs;
        private Dictionary<WindowType, WindowConfig> _windowConfigs;
        private Dictionary<BuildingType, StoreItemConfig> _storeItemsConfigs;

        public StaticDataService(IAssetProvider assetsProvider) =>
            _assetsProvider = assetsProvider;

        public GroundsConfig GroundsConfig { get; private set; }
        public StoreItemsConfig StoreItemsConfig { get; private set; }
        public WindowsConfig WindowsConfig { get; private set; }

        public async UniTask InitializeAsync()
        {
            List<UniTask> tasks = new List<UniTask>();

            tasks.Add(LoadBuildingConfigs());
            tasks.Add(LoadGroundsConfig());
            tasks.Add(LoadWindowsConfig());
            tasks.Add(LoadStoreItemsConfig());

            await UniTask.WhenAll(tasks);
        }

        public StoreItemConfig GetStoreItem(BuildingType buildingType) =>
            _storeItemsConfigs.TryGetValue(buildingType, out StoreItemConfig config) ? config : null;


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

        private async UniTask LoadStoreItemsConfig()
        {
            StoreItemsConfig[] storeItemsConfigs = await GetConfigs<StoreItemsConfig>();

            StoreItemsConfig = storeItemsConfigs.First();

            _storeItemsConfigs = StoreItemsConfig.Configs.ToDictionary(value => value.BuildingType, value => value);
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