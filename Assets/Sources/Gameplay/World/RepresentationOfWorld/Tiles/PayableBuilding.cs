using Assets.Sources.Gameplay.World.WorldInfrastructure;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using System;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles
{
    public class PayableBuilding : Building
    {
        private IStaticDataService _staticDataService;
        private MoveCounter _moveCounter;
        private IPersistentProgressService _persistentProgressService;
        private uint _payment;

        [Inject]
        private void Construct(IStaticDataService staticDataService, MoveCounter moveCounter, IPersistentProgressService persistentProgressService)
        {
            _staticDataService = staticDataService;
            _moveCounter = moveCounter;
            _persistentProgressService = persistentProgressService;

            _moveCounter.TimeToPaymentPayableBuildings += OnTimeToPaymentPayableBuildings;
        }

        private void OnDestroy()
        {
            _moveCounter.TimeToPaymentPayableBuildings -= OnTimeToPaymentPayableBuildings;
        }

        public override void Init(BuildingType type)
        {
            base.Init(type);

            _payment = _staticDataService.GetBuilding<PayableBuildingConfig>(Type).Payment;
        }

        private void OnTimeToPaymentPayableBuildings()
        {
            _persistentProgressService.Progress.WorldWallet.Give(_payment);
        }
    }
}
