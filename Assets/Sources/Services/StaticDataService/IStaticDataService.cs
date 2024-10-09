using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Services.StaticDataService
{
    public interface IStaticDataService
    {
        GroundsConfig GroundsConfig { get; }
        WindowsConfig WindowsConfig { get; }
        StoreItemsConfig StoreItemsConfig { get; }
        WorldsConfig WorldsConfig { get; }
        AvailableForConstructionBuildingsConfig AvailableForConstructionBuildingsConfig { get; }

        RoadConfig GetRoad(GroundType groundType, RoadType roadType);
        TBuilding GetBuilding<TBuilding>(BuildingType buildingType)
            where TBuilding : BuildingConfig;
        UniTask InitializeAsync();
        WindowConfig GetWindow(WindowType type);
        StoreItemConfig GetStoreItem(BuildingType buildingType);
        TestGroundConfig GetGround(TileType tileType);
        GroundType GetGroundType(BuildingType buildingType);
    }
}