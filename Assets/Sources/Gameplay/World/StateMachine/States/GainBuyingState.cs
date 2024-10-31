﻿using Assets.Sources.Services.StateMachine;
using Assets.Sources.Services.StaticDataService.Configs.WorldStore;
using Assets.Sources.UI;
using Assets.Sources.UI.Windows.World;
using Assets.Sources.UI.Windows.World.Panels.Store;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.StateMachine.States
{
    public class GainBuyingState : IPayloadState<GainStoreItemType>
    {
        private readonly UnlimitedQuantityGainBuyer _gainBuyer;
        private readonly WindowsSwitcher _windowsSwitcher;

        public GainBuyingState(UnlimitedQuantityGainBuyer gainBuyer, WindowsSwitcher windowsSwitcher)
        {
            _gainBuyer = gainBuyer;
            _windowsSwitcher = windowsSwitcher;
        }

        public UniTask Enter(GainStoreItemType gainStoreItemType)
        {
            Debug.Log("open");
            _gainBuyer.SetBuyingGainType(gainStoreItemType);
            _windowsSwitcher.Switch<GainBuyingWindow>("gain buyin");
            return default;
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
