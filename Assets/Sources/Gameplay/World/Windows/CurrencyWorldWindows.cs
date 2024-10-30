using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.World;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.Windows
{
    public class CurrencyWorldWindows : WorldWindows, IWorldWindows
    {
        public CurrencyWorldWindows(
            IPersistentProgressService persistentProgressService,
            WindowsSwitcher windowsSwitcher,
            IUiFactory uiFactory)
            : base(persistentProgressService, windowsSwitcher, uiFactory)
        {
        }

        protected override WindowType GameplayWindowType => WindowType.CurrencyGameplay;

        public override async UniTask Register()
        {
            await base.Register();

            await WindowsSwitcher.RegisterWindow<StoreWindow>(WindowType.WorldStore, UiFactory);
            await WindowsSwitcher.RegisterWindow<GainBuyingWindow>(WindowType.GainBuying, UiFactory);
        }

        public override void Remove()
        {
            base.Remove();

            WindowsSwitcher.Remove<StoreWindow>();
            WindowsSwitcher.Remove<GainBuyingWindow>();
        }
    }
}
