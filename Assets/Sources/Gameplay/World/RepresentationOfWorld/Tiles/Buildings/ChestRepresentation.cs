using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Buildings
{
    public class ChestRepresentation : BuildingRepresentation, IPointerClickHandler
    {
        private GameplayMover.GameplayMover _gameplayMover;
        private Vector2Int _gridPosition;
        private uint _reward;

        [Inject]
        private void Construct(GameplayMover.GameplayMover gameplayMover)
        {
            _gameplayMover = gameplayMover;
        }

        public void Init(Vector2Int gridPosition, uint reward)
        {
            _gridPosition = gridPosition;
            _reward = reward;
        }

        public void OnPointerClick(PointerEventData eventData) =>
            _gameplayMover.OpenChest(_gridPosition, _reward);
    }
}
