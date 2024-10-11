using System.Collections.Generic;
using System.Linq;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.Camera;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace Assets.Sources.Services.StaticDataService
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IAssetProvider _assetsProvider;

        private Dictionary<BuildingType, BuildingConfig> _buildingConfigs;
        private Dictionary<GroundType, Dictionary<RoadType, RoadConfig>> _groundConfigs;
        private Dictionary<WindowType, WindowConfig> _windowConfigs;
        private Dictionary<BuildingType, StoreItemConfig> _storeItemsConfigs;
        private Dictionary<TileType, TestGroundConfig> _testGroundConfigs;
        private Dictionary<BuildingType, GroundType> _roadGrounds;
        private Dictionary<GameplayCameraType, GameplayCameraConfig> _cameras;

        public StaticDataService(IAssetProvider assetsProvider) =>
            _assetsProvider = assetsProvider;

        public GroundsConfig GroundsConfig { get; private set; }
        public StoreItemsConfig StoreItemsConfig { get; private set; }
        public WindowsConfig WindowsConfig { get; private set; }
        public WorldsConfig WorldsConfig { get; private set; }
        public AvailableForConstructionBuildingsConfig AvailableForConstructionBuildingsConfig { get; private set; }
        public ReadOnlyArray<GameplayCameraConfig> CameraConfigs { get; private set; }

        public async UniTask InitializeAsync()
        {
            List<UniTask> tasks = new ();

            tasks.Add(LoadBuildingConfigs());
            tasks.Add(LoadGroundsConfig());
            tasks.Add(LoadWindowsConfig());
            tasks.Add(LoadStoreItemsConfig());
            tasks.Add(LoadWorldsConfig());
            tasks.Add(LoadRoadGroundConfigs());
            tasks.Add(LoadAvailableForConstructionBuildingsConfig());
            tasks.Add(LoadCameraConfigs());

            await UniTask.WhenAll(tasks);
        }

        public GameplayCameraConfig GetGameplayCamera(GameplayCameraType type) =>
            _cameras.TryGetValue(type, out GameplayCameraConfig config) ? config : null;

        public GroundType GetGroundType(BuildingType buildingType) =>
            _roadGrounds.TryGetValue(buildingType, out GroundType type) ? type : default;

        public TestGroundConfig GetGround(TileType tileType) =>
            _testGroundConfigs.TryGetValue(tileType, out TestGroundConfig config) ? config : null;

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

        private async UniTask LoadCameraConfigs()
        {
            CameraConfigs = await GetConfigs<GameplayCameraConfig>();

            _cameras = CameraConfigs.ToDictionary(value => value.Type, value => value);
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