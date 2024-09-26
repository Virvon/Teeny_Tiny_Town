using Assets.Sources.Gameplay.World;
using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Grounds;
using Assets.Sources.Services.AssetManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.GameplayFactory
{
    public class GameplayFactoryInstaller : Installer<GameplayFactoryInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IGameplayFactory>()
                .To<GameplayFactory>()
                .AsSingle();

            Container
                .BindFactory<string, Vector3, Transform, UniTask<TileRepresentation>, TileRepresentation.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<TileRepresentation>>();

            Container
                .BindFactory<AssetReferenceGameObject, Vector3, Transform, UniTask<Building>, Building.Factory>()
                .FromFactory<RefefencePrefabFactoryAsync<Building>>();

            Container
                .BindFactory<string, UniTask<SelectFrame>, SelectFrame.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<SelectFrame>>();

            Container
                .BindFactory<AssetReferenceGameObject, Vector3, float, Transform, UniTask<Ground>, Ground.Factory>()
                .FromFactory<RefefencePrefabFactoryAsync<Ground>>();

            Container
                .BindFactory<string, UniTask<BuildingMarker>, BuildingMarker.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<BuildingMarker>>();

            Container
                .BindFactory<string, UniTask<WorldsList>, WorldsList.Factory>()
                .FromFactory<KeyPrefabFactoryAsync<WorldsList>>();

            Container
               .BindFactory<string, Vector3, Transform, UniTask<World>, World.Factory>()
               .FromFactory<KeyPrefabFactoryAsync<World>>();
        }
    }
}
