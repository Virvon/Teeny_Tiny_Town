using System;
using UnityEngine;

namespace Assets.Sources.Data
{
    [Serializable]
    public class GameplayMovesCounterData
    {
        public uint RemainingMovesCount;
        
        public GameplayMovesCounterData(uint startRemainingMoveCount)
        {
            RemainingMovesCount = startRemainingMoveCount;
        }

        public event Action RemainingMovesCountChanged;
        public event Action MovesOvered;

        public bool CanMove => RemainingMovesCount != 0;

        public void Move()
        {
            if (RemainingMovesCount == 0)
            {
                Debug.LogError("No remain moves");
                return;
            }

            RemainingMovesCount--;

            RemainingMovesCountChanged?.Invoke();

            if (CanMove == false)
                MovesOvered?.Invoke();
        }

        public void SetCount(uint count)
        {
            RemainingMovesCount = count;
            RemainingMovesCountChanged?.Invoke();
        }
    }
}
