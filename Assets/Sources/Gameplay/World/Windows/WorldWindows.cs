using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.World;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.Windows
{
    public class WorldWindows : IWorldWindows
    {
        protected readonly WindowsSwitcher WindowsSwitcher;
        protected readonly IUiFactory UiFactory;

        private readonly IPersistentProgressService _persistentProgressService;

        public WorldWindows(IPersistentProgressService persistentProgressService, WindowsSwitcher windowsSwitcher, IUiFactory uiFactory)
        {
            _persistentProgressService = persistentProgressService;
            WindowsSwitcher = windowsSwitcher;
            UiFactory = uiFactory;

            IsRegistered = false;
        }

        public bool IsRegistered { get; private set; }
        protected virtual WindowType GameplayWindowType => WindowType.Gameplay;

        public virtual async UniTask Register()
        {
            await WindowsSwitcher.RegisterWindow<AdditionalBonusOfferWindow>(WindowType.AdditionalBonusOffer, UiFactory);
            await WindowsSwitcher.RegisterWindow<GameplayWindow>(GameplayWindowType, UiFactory);
            await WindowsSwitcher.RegisterWindow<RewardWindow>(WindowType.Reward, UiFactory);
            await WindowsSwitcher.RegisterWindow<ResultWindow>(WindowType.Result, UiFactory);
            await WindowsSwitcher.RegisterWindow<QuestsWindow>(WindowType.Quests, UiFactory);
            await WindowsSwitcher.RegisterWindow<SaveGameplayWindow>(WindowType.SaveGameplay, UiFactory);

            if (_persistentProgressService.Progress.StoreData.IsInfinityMovesUnlocked == false)
                await WindowsSwitcher.RegisterWindow<WaitingWindow>(WindowType.Waiting, UiFactory);

            IsRegistered = true;
        }

        public virtual void Remove()
        {
            WindowsSwitcher.Remove<AdditionalBonusOfferWindow>();
            WindowsSwitcher.Remove<GameplayWindow>();
            WindowsSwitcher.Remove<RewardWindow>();
            WindowsSwitcher.Remove<ResultWindow>();
            WindowsSwitcher.Remove<QuestsWindow>();
            WindowsSwitcher.Remove<SaveGameplayWindow>();

            if (_persistentProgressService.Progress.StoreData.IsInfinityMovesUnlocked == false)
                WindowsSwitcher.Remove<WaitingWindow>();

            IsRegistered = false;
        }
    }
}
