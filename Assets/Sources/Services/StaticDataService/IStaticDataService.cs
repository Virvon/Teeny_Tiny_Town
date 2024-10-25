using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.Quests;
using Assets.Sources.Services.StaticDataService.Configs.Reward;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem.Utilities;

namespace Assets.Sources.Services.StaticDataService
{
    public interface IStaticDataService
    {
        GroundsConfig GroundsConfig { get; }
        WindowsConfig WindowsConfig { get; }
        GameplayStoreItemsConfig StoreItemsConfig { get; }
        WorldsConfig WorldsConfig { get; }
        AvailableForConstructionBuildingsConfig AvailableForConstructionBuildingsConfig { get; }
        ReadOnlyArray<WorldConfig> WorldConfigs { get; }
        AnimationsConfig AnimationsConfig { get; }
        QuestsConfig QuestsConfig { get; }

        RoadConfig GetRoad(GroundType groundType, RoadType roadType);
        TBuilding GetBuilding<TBuilding>(BuildingType buildingType)
            where TBuilding : BuildingConfig;
        UniTask InitializeAsync();
        WindowConfig GetWindow(WindowType type);
        GameplayStoreItemConfig GetWorldStoreItem(BuildingType buildingType);
        TestGroundConfig GetGround(TileType tileType);
        GroundType GetGroundType(BuildingType buildingType);
        TWorldConfig GetWorld<TWorldConfig>(string id)
            where TWorldConfig : WorldConfig;
        RewardConfig GetReward(RewardType type);
        StoreItemConfig GetGameplayStorItem(GameplayStoreItemType type);
    }
}