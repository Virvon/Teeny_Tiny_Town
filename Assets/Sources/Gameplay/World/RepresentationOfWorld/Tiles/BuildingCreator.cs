using Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Grounds;
using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Infrastructure.Factories.GameplayFactory;
using Assets.Sources.Infrastructure.Factories.WorldFactory;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles
{
    public class BuildingCreator : MonoBehaviour
    {
        [SerializeField] private GroundCreator _groundCreator;

        private IWorldFactory _worldFactory;

        [Inject]
        private void Construct(IWorldFactory worldFactory) =>
            _worldFactory = worldFactory;

        private Transform BuildingPoint => _groundCreator.Ground.BuildingPoint;

        public async UniTask<Building> Create(BuildingType buildingType) =>
            await _worldFactory.CreateBuilding(buildingType, BuildingPoint.position, transform);
    }
}
