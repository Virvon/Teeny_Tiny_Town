using Assets.Sources.Data.WorldDatas;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.AdditionalBonuses
{
    [CreateAssetMenu(fileName = "BuldozerAdditionalBonusConfig", menuName = "StaticData/AdditionalBonus/Create new buldozer additional bonus config", order = 51)]
    public class BuldozerAdditionalBonusConfig : AdditionalBonusConfig
    {
        public override AdditionalBonusType Type => AdditionalBonusType.Buldozer;

        public override void Anwenden(IWorldData worldData) =>
            worldData.BuldozerItems.AddItems(Count);
    }
}