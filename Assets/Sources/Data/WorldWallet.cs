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

        public bool TryGet(uint value)
        {
            if(value > Value)
                return false;

            Value -= value;

            return true;
        }
    }
}
