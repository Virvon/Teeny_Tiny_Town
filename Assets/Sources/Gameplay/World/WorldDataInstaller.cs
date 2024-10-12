using Assets.Sources.Data;
using Assets.Sources.Services.PersistentProgress;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World
{
    public class WorldDataInstaller : MonoInstaller
    {
        private IPersistentProgressService _persistentProgressService;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService) =>
            _persistentProgressService = persistentProgressService;

        protected WorldData WorldData => _persistentProgressService.Progress.CurrentWorldData;

        public override void InstallBindings()
        {
            BindWorldData();
        }

        protected virtual void BindWorldData()
        {
            Container.BindInstance(_persistentProgressService.Progress.CurrentWorldData).AsSingle();
        }
    }
}
