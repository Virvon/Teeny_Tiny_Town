using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World
{
    public class World : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<string, Vector3, Transform, UniTask<World>>
        {
        }
    }
}
