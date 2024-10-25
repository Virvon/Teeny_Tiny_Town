using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.World;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World
{
    public class WorldWindows
    {
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly WindowsSwitcher _windowsSwitcher;
        private readonly IUiFactory _uiFactory;

        public WorldWindows(IPersistentProgressService persistentProgressService, WindowsSwitcher windowsSwitcher, IUiFactory uiFactory)
        {
            _persistentProgressService = persistentProgressService;
            _windowsSwitcher = windowsSwitcher;
            _uiFactory = uiFactory;

            IsRegistered = false;
        }

        public bool IsRegistered { get; private set; }

        public async UniTask Register()
        {
            await _windowsSwitcher.RegisterWindow<AdditionalBonusOfferWindow>(WindowType.AdditionalBonusOffer, _uiFactory);
            await _windowsSwitcher.RegisterWindow<GameplayWindow>(WindowType.Gameplay, _uiFactory);
            await _windowsSwitcher.RegisterWindow<RewardWindow>(WindowType.Reward, _uiFactory);
            await _windowsSwitcher.RegisterWindow<ResultWindow>(WindowType.Result, _uiFactory);
            await _windowsSwitcher.RegisterWindow<QuestsWindow>(WindowType.Quests, _uiFactory);
            await _windowsSwitcher.RegisterWindow<SaveGameplayWindow>(WindowType.SaveGameplay, _uiFactory);

            if (_persistentProgressService.Progress.StoreData.IsInfinityMovesUnlocked == false)
                await _windowsSwitcher.RegisterWindow<WaitingWindow>(WindowType.Waiting, _uiFactory);

            IsRegistered = true;
        }

        public void Remove()
        {
            _windowsSwitcher.Remove<AdditionalBonusOfferWindow>();
            _windowsSwitcher.Remove<GameplayWindow>();
            _windowsSwitcher.Remove<RewardWindow>();
            _windowsSwitcher.Remove<ResultWindow>();
            _windowsSwitcher.Remove<QuestsWindow>();

            if (_persistentProgressService.Progress.StoreData.IsInfinityMovesUnlocked == false)
                _windowsSwitcher.Remove<WaitingWindow>();

            IsRegistered = false;
        }
    }
}
