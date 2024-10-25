using System;
using UnityEngine;

namespace Assets.Sources.Data
{
    [Serializable]
    public class GameplayMovesCounterData
    {
        public uint RemainingMovesCount;
        public StoreData StoreData;

        public GameplayMovesCounterData(uint startRemainingMoveCount, StoreData storeData)
        {
            RemainingMovesCount = startRemainingMoveCount;
            StoreData = storeData;
        }

        public event Action RemainingMovesCountChanged;
        public event Action MovesOvered;

        public bool CanMove => RemainingMovesCount != 0 || StoreData.IsInfinityMovesUnlocked;

        public void Move()
        {
            if (StoreData.IsInfinityMovesUnlocked)
                return;

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
            if (StoreData.IsInfinityMovesUnlocked)
                return;

            RemainingMovesCount = count;
            RemainingMovesCountChanged?.Invoke();
        }
    }
}
