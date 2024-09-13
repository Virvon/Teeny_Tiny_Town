using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Tile
{
    public class Tile : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<string, Vector3, Transform, UniTask<Tile>>
        {
        }
    }
}
