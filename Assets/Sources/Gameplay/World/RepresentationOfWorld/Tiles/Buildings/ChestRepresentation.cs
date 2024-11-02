using Assets.Sources.Gameplay.GameplayMover;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Buildings
{
    public class ChestRepresentation : BuildingRepresentation, IPointerClickHandler
    {
        private IGameplayMover _gameplayMover;
        private Vector2Int _gridPosition;
        private uint _reward;

        [Inject]
        private void Construct(IGameplayMover gameplayMover = null)
        {
            _gameplayMover = gameplayMover;
        }

        public void Init(Vector2Int gridPosition, uint reward)
        {
            _gridPosition = gridPosition;
            _reward = reward;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(_gameplayMover != null)
                _gameplayMover.OpenChest(_gridPosition, _reward);
        }
    }
}
