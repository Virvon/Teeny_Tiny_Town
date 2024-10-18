using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Cameras
{
    public class GameplayCamera : MonoBehaviour
    {
        public void MoveTo(Vector3 position)
        {
            transform.DOMove(position, 1);
        }

        public class Factory : PlaceholderFactory<string, UniTask<GameplayCamera>>
        {
        }
    }
}
