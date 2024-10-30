using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.WorldChangers
{
    public interface IExpandingWorldChanger : IWorldChanger
    {
        UniTask Expand();
    }
}