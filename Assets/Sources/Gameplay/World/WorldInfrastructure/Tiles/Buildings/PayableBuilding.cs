using Assets.Sources.Data;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using UnityEngine;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles.Buildings
{
    public class PayableBuilding : Building
    {
        public readonly uint Payment;

        private readonly IStaticDataService _staticDataService;
        private readonly WorldWallet _worldWallet;
        private readonly IPersistentProgressService _persistentProgressService;

        public PayableBuilding(BuildingType type, IStaticDataService staticDataService, WorldWallet worldWallet, IPersistentProgressService persistentProgressService)
            : base(type)
        {
            _staticDataService = staticDataService;
            _worldWallet = worldWallet;
            _persistentProgressService = persistentProgressService;

            Payment = _staticDataService.GetBuilding<PayableBuildingConfig>(Type).Payment;

            _persistentProgressService.Progress.MoveCounter.TimeToPaymentPayableBuildings += OnTimeToPaymentPayableBuildings;
        }

        ~PayableBuilding() =>
            _persistentProgressService.Progress.MoveCounter.TimeToPaymentPayableBuildings -= OnTimeToPaymentPayableBuildings;

        private void OnTimeToPaymentPayableBuildings() =>
            _worldWallet.Give(Payment);
    }
}
