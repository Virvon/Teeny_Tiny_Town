using System;
using UnityEngine;

namespace Assets.Sources.Data
{
    [Serializable]
    public class WorldWallet
    {
        public uint Value;

        public event Action<uint> ValueChanged;

        public void Give(uint value)
        {
            Value += value;
            ValueChanged?.Invoke(Value);
        }

        public bool TryGet(uint value)
        {
            if(value > Value)
                return false;

            Value -= value;
            ValueChanged?.Invoke(Value);

            return true;
        }

        public void ForceGet(uint value)
        {
            if (value > Value)
            {
                Debug.LogError("Too much value to force get");
                Value = 0;

                return;
            }

            Value -= value;
            ValueChanged?.Invoke(Value);
        }
    }
}
