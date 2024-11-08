using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Grounds
{
    public class GroundCreator : MonoBehaviour
    {
        [SerializeField] private Transform _groundPoint;

        private IWorldFactory _worldFactory;
        private AnimationsConfig _animationsConfig;

        public Ground Ground { get; private set; }

        [Inject]
        private void Construct(IWorldFactory worldFactory, IStaticDataService staticDataService)
        {
            _worldFactory = worldFactory;
            _animationsConfig = staticDataService.AnimationsConfig;
        }

        public async UniTask Create(TileType tileType)
        {
            if (Ground != null)
                Destroy(Ground.gameObject);

            Ground = await _worldFactory.CreateGround(tileType, _groundPoint.position, transform);
        }

        public async UniTask Create(GroundType groundType, RoadType roadType, GroundRotation rotation, bool isAnimate)
        {
            if (Ground != null)
                Destroy(Ground.gameObject);

            Ground = await _worldFactory.CreateGround(groundType, roadType, _groundPoint.position, rotation, transform);

            if (isAnimate)
                await UniTask.WaitForSeconds(_animationsConfig.TileUpdatingDuration);
        }
    }
}
