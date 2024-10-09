using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;
using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.Tiles.Buildings
{
    public class PayableBuilding : Building
    {
        private IStaticDataService _staticDataService;
        private MoveCounter _moveCounter;
        private World _world;
        private uint _payment;

        [Inject]
        private void Construct(IStaticDataService staticDataService, MoveCounter moveCounter, World world)
        {
            _staticDataService = staticDataService;
            _moveCounter = moveCounter;
            _world = world;

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
            _world.WorldData.WorldWallet.Give(_payment);
        }
    }
}
