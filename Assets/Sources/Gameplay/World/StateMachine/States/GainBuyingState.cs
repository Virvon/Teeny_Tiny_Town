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

        public async UniTask Enter(GainStoreItemType gainStoreItemType)
        {
            _gainBuyer.SetBuyingGainType(gainStoreItemType);
            await _windowsSwitcher.Switch<GainBuyingWindow>();
        }

        public UniTask Exit() =>
            default;
    }
}
