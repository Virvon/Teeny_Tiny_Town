using Assets.Sources.Data;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers
{
    public interface IExpandingWorldChanger : IWorldChanger
    {
        UniTask Expand();
    }
}