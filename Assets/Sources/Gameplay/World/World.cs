using Assets.Sources.Gameplay.World.Root;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay.World
{
    public class World : MonoBehaviour
    {
        [SerializeField] protected WorldInstaller WorldInstaller;

        public virtual void EnterBootstrapState()
        {
            WorldInstaller.WorldStateMachine.Enter<WorldBootstrapState>().Forget();
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Transform, UniTask<World>>
        {
        }
    }
}
