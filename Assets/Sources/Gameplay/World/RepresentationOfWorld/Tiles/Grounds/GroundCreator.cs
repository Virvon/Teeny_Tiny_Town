using Assets.Sources.Infrastructure.Factories.WorldFactory;
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

        [Inject]
        private void Construct(IWorldFactory worldFactory)
        {
            _worldFactory = worldFactory;
        }

        public Ground Ground { get; private set; }

        public async UniTask Create(TileType tileType)
        {
            if (Ground != null)
                Destroy(Ground.gameObject);

            Ground = await _worldFactory.CreateGround(tileType, _groundPoint.position, transform);
        }

        public async UniTask Create(GroundType groundType, RoadType roadType, GroundRotation rotation)
        {
            if (Ground != null)
                Destroy(Ground.gameObject);

            Ground = await _worldFactory.CreateGround(groundType, roadType, _groundPoint.position, rotation, transform);
        }
    }
}
