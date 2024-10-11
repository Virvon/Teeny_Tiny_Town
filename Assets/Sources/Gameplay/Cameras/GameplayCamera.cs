using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay.Cameras
{
    public class GameplayCamera : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

        public void SetPriority(int value) =>
            _cinemachineVirtualCamera.Priority = value;

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, UniTask<GameplayCamera>>
        {
        }
    }
}
