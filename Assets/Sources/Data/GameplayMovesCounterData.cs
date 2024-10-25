using System;
using UnityEngine;

namespace Assets.Sources.Data
{
    [Serializable]
    public class GameplayMovesCounterData
    {
        public uint RemainingMovesCount;

        public event Action RemainingMovesCountChanged;

        public GameplayMovesCounterData(uint startRemainingMoveCount)
        {
            RemainingMovesCount = startRemainingMoveCount;
        }

        public void Move()
        {
            if (RemainingMovesCount == 0)
            {
                Debug.LogError("No remain moves");
                return;
            }

            RemainingMovesCount--;

            RemainingMovesCountChanged?.Invoke();
        }

        public void SetCount(uint count)
        {
            RemainingMovesCount = count;
            RemainingMovesCountChanged?.Invoke();
        }
    }
}
