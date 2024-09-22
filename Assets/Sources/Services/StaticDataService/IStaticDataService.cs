using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Services.StaticDataService
{
    public interface IStaticDataService
    {
        GroundsConfig GroundsConfig { get; }

        RoadConfig GetRoad(GroundType groundType, RoadType roadType);
        BuildingConfig GetBuilding(BuildingType buildingType);
        UniTask InitializeAsync();
    }
}