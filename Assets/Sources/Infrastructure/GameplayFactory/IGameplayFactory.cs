using Cysharp.Threading.Tasks;

namespace Assets.Sources.Infrastructure.GameplayFactory
{
    public interface IGameplayFactory
    {
        UniTask CreateWorldGenerator();
    }
}
