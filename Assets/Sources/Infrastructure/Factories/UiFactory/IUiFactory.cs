using Assets.Sources.Services.StaticDataService.Configs.AdditionalBonuses;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Assets.Sources.Services.StaticDataService.Configs.Reward;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.Services.StaticDataService.Configs.WorldStore;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.World.Panels;
using Assets.Sources.UI.Windows.World.Panels.Reward;
using Assets.Sources.UI.Windows.World.Panels.Store;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.UiFactory
{
    public interface IUiFactory
    {
        UniTask CreateAdditionBonusOfferItem(AdditionalBonusType type, Transform parent);
        UniTask CreateGainStoreItemPanel(GainStoreItemType type, Transform parent);
        UniTask<QuestPanel> CreateQuestPanel(string id, Transform parent);
        UniTask CreateRemainingMovesPanel(Transform parent);
        UniTask<RewardPanel> CreateRewardPanel(RewardType type, Transform parent);
        UniTask<BuildingStoreItem> CreateStoreItem(BuildingType buildingType, Transform parent);
        UniTask<Window> CreateWindow(WindowType type);
    }
}