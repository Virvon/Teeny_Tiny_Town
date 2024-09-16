using Assets.Sources.Infrastructure.GameplayFactory;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Tile
{
    public class BuildingCreator : MonoBehaviour
    {
        [SerializeField] private GroundCreator _groundCreator;

        private IGameplayFactory _gameplayFactory;

        [Inject]
        private void Construct(IGameplayFactory gameplayFactory)
        {
            _gameplayFactory = gameplayFactory;
        }

        private Transform BuildingPoint => _groundCreator.Ground.BuildingPoint;

        public async UniTask<Building> Create(BuildingType buildingType)
        {
            return await _gameplayFactory.CreateBuilding(buildingType, BuildingPoint.position, transform);
        }
    }
}
