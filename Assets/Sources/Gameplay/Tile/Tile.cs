using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Tile
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private TileSelection _tileSelection;
        [SerializeField] private TileMerge _tileMerge;

        public TileSelection TileSelection => _tileSelection;
        public TileMerge TileMerge => _tileMerge;

        public class Factory : PlaceholderFactory<string, Vector3, Transform, UniTask<Tile>>
        {
        }
    }
}
