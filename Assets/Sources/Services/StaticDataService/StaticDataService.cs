using System.Collections.Generic;
using System.Linq;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IAssetProvider _assetsProvider;

        private Dictionary<BuildingType, BuildingConfig> _buildingConfigs;
        private Dictionary<GroundType, Dictionary<RoadType, RoadConfig>> _groundConfigs;

        public StaticDataService(IAssetProvider assetsProvider) =>
            _assetsProvider = assetsProvider;

        public GroundsConfig GroundsConfig { get; private set; }

        public async UniTask InitializeAsync()
        {
            List<UniTask> tasks = new List<UniTask>();

            tasks.Add(LoadBuildingConfigs());
            tasks.Add(LoadGroundsConfig());

            await UniTask.WhenAll(tasks);
        }

        public BuildingConfig GetBuilding(BuildingType buildingType) =>
            _buildingConfigs.TryGetValue(buildingType, out BuildingConfig config) ? config : null;

        public RoadConfig GetRoad(GroundType groundType, RoadType roadType)
        {
            return _groundConfigs.TryGetValue(
                groundType, out Dictionary<RoadType, RoadConfig> roadConfigs) ? (roadConfigs.TryGetValue(
                roadType, out RoadConfig config) ? config : null) : null;
        }

        private async UniTask LoadGroundsConfig()
        {
            GroundsConfig[] groundsConfig = await GetConfigs<GroundsConfig>();

            GroundsConfig = groundsConfig.First();
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