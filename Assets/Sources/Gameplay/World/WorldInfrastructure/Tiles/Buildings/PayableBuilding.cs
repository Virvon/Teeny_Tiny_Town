using Assets.Sources.Data.WorldDatas;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Building;

namespace Assets.Sources.Gameplay.World.WorldInfrastructure.Tiles.Buildings
{
    public class PayableBuilding : Building
    {
        public readonly uint Payment;

        private readonly IStaticDataService _staticDataService;
        private readonly WorldWallet _worldWallet;
        private readonly ICurrencyWorldData _currencyWorldData;

        public PayableBuilding(BuildingType type, IStaticDataService staticDataService, WorldWallet worldWallet, ICurrencyWorldData currencyWorldData)
            : base(type)
        {
            _staticDataService = staticDataService;
            _worldWallet = worldWallet;
            _currencyWorldData = currencyWorldData;

            Payment = _staticDataService.GetBuilding<PayableBuildingConfig>(Type).Payment;

            _currencyWorldData.MovesCounter.TimeToPaymentPayableBuildings += OnTimeToPaymentPayableBuildings;
        }

        ~PayableBuilding() =>
            _currencyWorldData.MovesCounter.TimeToPaymentPayableBuildings -= OnTimeToPaymentPayableBuildings;

        private void OnTimeToPaymentPayableBuildings() =>
            _worldWallet.Give(Payment);
    }
}
