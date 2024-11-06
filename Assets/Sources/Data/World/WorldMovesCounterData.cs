using System;

namespace Assets.Sources.Data.World
{
    [Serializable]
    public class WorldMovesCounterData
    {
        private const uint MovesCountToBuildingsPayment = 5;

        public uint MovesCount;

        public event Action TimeToPaymentPayableBuildings;

        public void Move()
        {
            MovesCount++;

            if (MovesCount % MovesCountToBuildingsPayment == 0)
                TimeToPaymentPayableBuildings?.Invoke();
        }
    }
}
