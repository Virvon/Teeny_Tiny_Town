using Assets.Sources.Data.World;
using Assets.Sources.Data.World.Currency;
using Assets.Sources.Gameplay.GameplayMover;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.Windows;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.UI.Windows.World.Panels.Store;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.Root
{
    public class CurrencyWorldInstaller : WorldInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();

            Container.Bind<GainBuyer>().AsSingle();
            Container.Bind<UnlimitedQuantityGainBuyer>().AsSingle();
            GameplayFactoryInstaller.Install(Container);
        }

        protected override void BindAcitonHandlerSwitcher()
        {
            Container.BindInterfacesTo<CurrencyWorldActionHandlerSwitcher>().AsSingle();
        }

        protected override void BindWorldWindows()
        {
            Container.BindInterfacesTo<CurrencyWorldWindows>().AsSingle();
        }

        protected override void BindWorldBootstrapper()
        {
            Container.BindInterfacesAndSelfTo<CurrencyWorldBootstrapper>().AsSingle().NonLazy();
        }

        protected override void BindWorldData()
        {
            WorldData worldData = PersistentProgressService.Progress.GetWorldData(WorldDataId);

            if (worldData is not CurrencyWorldData)
                Debug.LogError(nameof(WorldDataId) + " is not " + typeof(CurrencyWorldData));

            Container.BindInterfacesTo<CurrencyWorldData>().FromInstance(worldData as CurrencyWorldData).AsSingle();
        }

        protected override void BindGameplayMover()
        {
            Container.BindInterfacesTo<CurrencyGameplayMover>().AsSingle();
        }

        protected override void BindWorldChanger()
        {
            Container.BindInterfacesTo<CurrencyWorldChanger>().AsSingle();
        }
    }

}
