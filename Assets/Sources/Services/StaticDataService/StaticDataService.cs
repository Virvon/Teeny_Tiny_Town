﻿using System.Collections.Generic;
using System.Linq;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Services.AssetManagement;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Services.StaticDataService
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IAssetProvider _assetsProvider;

        private Dictionary<BuildingType, MergeConfig> _mergeConfigs;

        public StaticDataService(IAssetProvider assetsProvider) =>
            _assetsProvider = assetsProvider;

        public async UniTask InitializeAsync()
        {
            List<UniTask> tasks = new List<UniTask>();

            tasks.Add(LoadMergeConfig());

            await UniTask.WhenAll(tasks);
        }

        public MergeConfig GetMerge(BuildingType buildingType) =>
            _mergeConfigs.TryGetValue(buildingType, out MergeConfig config) ? config : null;

        private async UniTask LoadMergeConfig()
        {
            MergeConfig[] mergeConfigs = await GetConfigs<MergeConfig>();

            _mergeConfigs = mergeConfigs.ToDictionary(value => value.BuildingType, value => value);
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