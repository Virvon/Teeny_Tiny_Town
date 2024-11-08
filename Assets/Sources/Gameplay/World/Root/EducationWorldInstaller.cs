using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.Windows;

namespace Assets.Sources.Gameplay.World.Root
{
    public class EducationWorldInstaller : CurrencyWorldInstaller
    {
        protected override void BindWorldBootstrapper() =>
            Container.BindInterfacesAndSelfTo<EducationWorldBootstrapper>().AsSingle().NonLazy();

        protected override void BindWorldWindows() =>
            Container.BindInterfacesTo<EducationWorldWindows>().AsSingle();

        protected override void BindAcitonHandlerSwitcher() =>
            Container.BindInterfacesTo<ActionHandlerSwitcher>().AsSingle();
    }
}