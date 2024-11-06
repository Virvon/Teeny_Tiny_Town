﻿using Assets.Sources.Data.World;
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
            WorldData worldData = PersistentProgressService.Progress.GetWorldData(WorldDataId);

            if (worldData is not ExpandingWorldData)
                Debug.LogError(nameof(WorldDataId) + " is not " + typeof(ExpandingWorldData));

            Container.BindInterfacesTo<ExpandingWorldData>().FromInstance(worldData as ExpandingWorldData).AsSingle();
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
            Container.BindInterfacesAndSelfTo<WorldExpander>().AsSingle().NonLazy();
        }
    }
}
