using Assets.Sources.Services.StaticDataService.Configs.Building;

namespace Assets.Sources.Gameplay.GameplayMover
{
    public interface ICurrencyGameplayMover : IGameplayMover
    {
        void ChangeBuildingForPlacing(BuildingType targetBuildingType, uint buildingPrice);
    }
}