using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Services.StaticDataService
{
    public interface IStaticDataService
    {
        GroundsConfig GroundsConfig { get; }
        WindowsConfig WindowsConfig { get; }
        StoreItemsConfig StoreItemsConfig { get; }

        RoadConfig GetRoad(GroundType groundType, RoadType roadType);
        TBuilding GetBuilding<TBuilding>(BuildingType buildingType)
            where TBuilding : BuildingConfig;
        UniTask InitializeAsync();
        WindowConfig GetWindow(WindowType type);
        StoreItemConfig GetStoreItem(BuildingType buildingType);
    }
}