using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld
{
    public interface ITileRepresentationCreatable
    {
        UniTask<TileRepresentation> Create(Vector2Int gridPosition);
    }
}