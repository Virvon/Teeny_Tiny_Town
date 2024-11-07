using System;
using UnityEngine;

namespace Assets.Sources.Data.World
{

    [Serializable]
    public class WorldWallet : Wallet
    {
        public uint StartValue;

        public WorldWallet(uint startValue)
            : base(startValue) => StartValue = startValue;

        public void ForceGet(uint value)
        {
            if (value > Value)
            {
                Debug.LogError("Too much value to force get");
                Value = 0;

                return;
            }

            Value -= value;

            OnValueChanged();
        }

        public void Clear()
        {
            Value = StartValue;
            OnValueChanged();
        }
    }
}
