using Assets.Sources.Gameplay.Store;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI.Windows;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.UiFactory
{
    public interface IUiFactory
    {
        UniTask<StoreItem> CreateStoreItem(BuildingType buildingType, Transform parent);
        UniTask<Window> CreateWindow(WindowType type);
    }
}