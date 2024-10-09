using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Buildings
{
    public class Chest : Building, IPointerClickHandler
    {
        private IStaticDataService _staticDataService;
        private WorldGenerator _worldGenerator;
        private GameplayMover.GameplayMover _gameplayMover;

        private uint _reward;
        private Vector2Int _gridPosition;

        [Inject]
        private void Construct(IStaticDataService staticDataService, WorldGenerator worldGenerator, GameplayMover.GameplayMover gameplayMover)
        {
            _staticDataService = staticDataService;
            _worldGenerator = worldGenerator;
            _gameplayMover = gameplayMover;
        }

        public override void Init(BuildingType type)
        {
            base.Init(type);

            _gridPosition = _worldGenerator.WorldToGridPosition(transform.position);
            _reward = _staticDataService.GetBuilding<ChestConfig>(Type).Reward;
        }

        public void OnPointerClick(PointerEventData eventData) =>
            _gameplayMover.OpenChest(_gridPosition, _reward);
    }
}
