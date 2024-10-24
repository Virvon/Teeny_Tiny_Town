using Assets.Sources.Gameplay.Store;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.Reward;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.World.Panels;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.UiFactory
{
    public interface IUiFactory
    {
        UniTask<RewardPanel> CreateRewardPanel(RewardType type, Transform parent);
        UniTask<StoreItem> CreateStoreItem(BuildingType buildingType, Transform parent);
        UniTask<Window> CreateWindow(WindowType type);
    }
}