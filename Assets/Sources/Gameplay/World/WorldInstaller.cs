using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World
{
    public class WorldInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindWorldBootstrapper();
            BindWorldChanger();
            BindWorldRepresentationChanger();
            BindWorldFactory();
            BindGameplayMover();
        }

        private void BindGameplayMover()
        {
            Container.BindInterfacesAndSelfTo<GameplayMover.GameplayMover>().AsSingle();
        }

        private void BindWorldFactory()
        {
            WorldFactoryInstaller.Install(Container);
        }

        private void BindWorldBootstrapper()
        {
            Container.BindInterfacesAndSelfTo<WorldBootstrapper>().AsSingle().NonLazy();
        }

        private void BindWorldChanger()
        {
            Container.BindInterfacesAndSelfTo<WorldChanger>().AsSingle();
        }

        private void BindWorldRepresentationChanger()
        {
            Container.BindInterfacesAndSelfTo<WorldRepresentationChanger>().AsSingle();
        }
    }
}
