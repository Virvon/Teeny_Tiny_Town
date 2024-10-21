using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Gameplay.GameplayMover;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.Root
{
    public class CurrencyWorldInstaller : WorldInstaller
    {
        protected override void BindWorldBootstrapper()
        {
            Container.BindInterfacesAndSelfTo<CurrencyWorldBootstrapper>().AsSingle().NonLazy();
        }

        protected override void BindWorldData()
        {
            if (WorldData is not CurrencyWorldData)
                Debug.LogError(nameof(WorldData) + " is not " + typeof(CurrencyWorldData));

            Container.BindInterfacesTo<CurrencyWorldData>().FromInstance(WorldData as CurrencyWorldData).AsSingle();
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
