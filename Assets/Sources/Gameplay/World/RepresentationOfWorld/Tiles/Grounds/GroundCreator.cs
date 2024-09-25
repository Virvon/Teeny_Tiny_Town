using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Services.StaticDataService.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Grounds
{
    public class GroundCreator : MonoBehaviour
    {
        [SerializeField] private Transform _groundPoint;

        private IGameplayFactory _gameplayFactory;

        [Inject]
        private void Construct(IGameplayFactory gameplayFactory)
        {
            _gameplayFactory = gameplayFactory;
        }

        public Ground Ground { get; private set; }

        public async UniTask Create(GroundType groundType, RoadType roadType, GroundRotation rotation)
        {
            if (Ground != null)
                Destroy(Ground.gameObject);

            Ground = await _gameplayFactory.CreateGround(groundType, roadType, _groundPoint.position, rotation, transform);
        }
    }
}
