using Assets.Sources.Data.World.Currency;
using Assets.Sources.Data.WorldDatas;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.AdditionalBonuses
{
    [CreateAssetMenu(fileName = "WorldWalletValueAddtionalBonusConfig", menuName = "StaticData/AdditionalBonus/Create new world wallet value additional bonus config", order = 51)]
    public class WorldWalletValueAddtionalBonusConfig : AdditionalBonusConfig
    {
        public override AdditionalBonusType Type => AdditionalBonusType.WorldWalletValue;

        public override void Anwenden(IWorldData worldData)
        {
            if(worldData is ICurrencyWorldData currencyWorldData)
                currencyWorldData.WorldWallet.Give(Count);
            else
                Debug.LogError($"{nameof(worldData)} is not {typeof(ICurrencyWorldData)}");
        }
    }
}