using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.WorldFactory
{
    public interface IWorldFactory
    {
        WorldGenerator WorldGenerator { get; }

        UniTask<WorldGenerator> CreateWorldGenerator(Transform parent);
    }
}