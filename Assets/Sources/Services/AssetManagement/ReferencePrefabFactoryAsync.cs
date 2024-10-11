using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Services.AssetManagement
{
    public class ReferencePrefabFactoryAsync<TComponent> : IFactory<AssetReferenceGameObject, UniTask<TComponent>>,
        IFactory<AssetReferenceGameObject, Vector3, Transform, UniTask<TComponent>>,
        IFactory<AssetReferenceGameObject, Vector3, float, Transform, UniTask<TComponent>>,
        IFactory<AssetReferenceGameObject, Transform, UniTask<TComponent>>
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IInstantiator _instantiator;

        public ReferencePrefabFactoryAsync(IAssetProvider assetProvider, IInstantiator instantiator)
        {
            _assetProvider = assetProvider;
            _instantiator = instantiator;
        }

        public async UniTask<TComponent> Create(AssetReferenceGameObject assetReference)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(assetReference);
            GameObject newObject = _instantiator.InstantiatePrefab(prefab);
            return newObject.GetComponent<TComponent>();
        }

        public async UniTask<TComponent> Create(AssetReferenceGameObject assetReference, Vector3 position, Transform parent)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(assetReference);
            GameObject newObject = _instantiator.InstantiatePrefab(prefab, position, Quaternion.identity, parent);
            return newObject.GetComponent<TComponent>();
        }

        public async UniTask<TComponent> Create(AssetReferenceGameObject assetReference, Vector3 position, float rotationY, Transform parent)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(assetReference);
            GameObject newObject = _instantiator.InstantiatePrefab(prefab, position, Quaternion.Euler(0, rotationY, 0), parent);
            return newObject.GetComponent<TComponent>();
        }

        public async UniTask<TComponent> Create(AssetReferenceGameObject assetReference, Transform parent)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(assetReference);
            GameObject newObject = _instantiator.InstantiatePrefab(prefab, parent);
            return newObject.GetComponent<TComponent>();
        }
    }
}