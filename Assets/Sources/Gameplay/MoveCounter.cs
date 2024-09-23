using System;

namespace Assets.Sources.Gameplay
{
    public class MoveCounter
    {
        private const uint MovesCountToBuildingsPayment = 5;

        private readonly GameplayMover.GameplayMover _gameplayMover;

        private uint _movesCount;

        public MoveCounter(GameplayMover.GameplayMover gameplayMover)
        {
            _gameplayMover = gameplayMover;

            _gameplayMover.GameplayMoved += OnGameplayMoved;
        }

        public event Action TimeToPaymentPayableBuildings;

        ~MoveCounter()
        {
            _gameplayMover.GameplayMoved -= OnGameplayMoved;
        }

        private void OnGameplayMoved()
        {
            _movesCount++;

            if (_movesCount % MovesCountToBuildingsPayment == 0)
                TimeToPaymentPayableBuildings?.Invoke();
        }
    }
}
