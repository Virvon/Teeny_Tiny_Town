using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class WorldWallet
    {
        public uint Value;

        public event Action<uint> ValueChanged;

        public void Give(uint payment)
        {
            Value += payment;

            ValueChanged?.Invoke(Value);
        }
    }
}
