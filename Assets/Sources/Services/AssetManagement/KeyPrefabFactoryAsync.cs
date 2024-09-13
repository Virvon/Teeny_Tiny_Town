using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Services.AssetManagement
{
    public class KeyPrefabFactoryAsync<TComponent> : IFactory<string, UniTask<TComponent>>, IFactory<string, Vector3, Transform, UniTask<TComponent>>
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IInstantiator _instantiator;

        public KeyPrefabFactoryAsync(IAssetProvider assetProvider, IInstantiator instantiator)
        {
            _assetProvider = assetProvider;
            _instantiator = instantiator;
        }

        public async UniTask<TComponent> Create(string assetKey)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(assetKey);
            GameObject newObject = _instantiator.InstantiatePrefab(prefab);
            return newObject.GetComponent<TComponent>();
        }

        public async UniTask<TComponent> Create(string assetKey, Vector3 position, Transform parent)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(assetKey);
            GameObject newObject = _instantiator.InstantiatePrefab(prefab, position, Quaternion.identity, parent);
            return newObject.GetComponent<TComponent>();
        }
    }
}