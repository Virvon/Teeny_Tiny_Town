using Assets.Sources.Gameplay.Store;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI;
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