using Assets.Sources.Gameplay.WorldGenerator;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Infrastructure.GameplayFactory
{
    public class GameplayFactory : IGameplayFactory
    {
        private readonly WorldGenerator.Factory _worldGeneratorFactory;

        public GameplayFactory(WorldGenerator.Factory worldGeneratorFactory)
        {
            _worldGeneratorFactory = worldGeneratorFactory;
        }

        public async UniTask CreateWorldGenerator()
        {
            await _worldGeneratorFactory.Create(GameplayFactoryAssets.WorldGenerator);
        }
    }
}
