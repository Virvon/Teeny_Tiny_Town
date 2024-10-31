using Assets.Sources.Data.World;
using Assets.Sources.Gameplay.ExpandingLogic;
using Assets.Sources.Gameplay.GameplayMover;
using Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.Root
{
    public class ExpandingWorldInstaller : CurrencyWorldInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();

            BindWorldExpander();
        }

        protected override void BindWorldData()
        {
            if (WorldData is not ExpandingWorldData)
                Debug.LogError(nameof(WorldData) + " is not " + typeof(ExpandingWorldData));

            Container.BindInterfacesTo<ExpandingWorldData>().FromInstance(WorldData as ExpandingWorldData).AsSingle();
        }

        protected override void BindGameplayMover()
        {
            Container.BindInterfacesTo<ExpandingGameplayMover>().AsSingle();
        }

        protected override void BindWorldChanger()
        {
            Container.BindInterfacesTo<ExpandingWorldChanger>().AsSingle();
        }

        private void BindWorldExpander()
        {
            Container.Bind<WorldExpander>().AsSingle().NonLazy();
        }
    }
}
