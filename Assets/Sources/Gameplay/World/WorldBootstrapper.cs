using Assets.Sources.Gameplay.World.RepresentationOfWorld;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Zenject;

namespace Assets.Sources.Gameplay.World
{
    public class WorldBootstrapper : IInitializable
    {
        private readonly WorldChanger _worldChanger;
        private readonly IWorldFactory _worldFactory;

        public WorldBootstrapper(WorldChanger worldChanger, IWorldFactory worldFactory)
        {
            _worldChanger = worldChanger;
            _worldFactory = worldFactory;
        }

        public async void Initialize()
        {
            WorldGenerator worldGenerator = await _worldFactory.CreateWorldGenerator(null);

            _worldChanger.Generate();
            await worldGenerator.Generate();
            _worldChanger.Work();
        }
    }
}
