using Assets.Sources.Gameplay.Tile;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Services.StaticDataService
{
    public interface IStaticDataService
    {
        MergeConfig GetMerge(BuildingType buildingType);
        UniTask InitializeAsync();
    }
}