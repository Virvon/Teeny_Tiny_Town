using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Assets.Sources.UI;

namespace Assets.Sources.Gameplay.World.Windows
{
    public class EducationWorldWindows : CurrencyWorldWindows
    {
        public EducationWorldWindows(IPersistentProgressService persistentProgressService, WindowsSwitcher windowsSwitcher, IUiFactory uiFactory)
            : base(persistentProgressService, windowsSwitcher, uiFactory)
        {
        }

        protected override WindowType GameplayWindowType => WindowType.Education;
    }
}
