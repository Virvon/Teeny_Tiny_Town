using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Tile
{
    public class Building : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<string, Vector3, Transform, UniTask<Building>>
        {
        }
    }
}
