using Assets.Sources.Data.World;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.AdditionalBonuses
{
    [CreateAssetMenu(fileName = "ReplaceItemAdditionalBonusConfig", menuName = "StaticData/AdditionalBonus/Create new replace item additional bonus config", order = 51)]
    public class ReplaceItemAdditionalBonusConfig : AdditionalBonusConfig
    {
        public override AdditionalBonusType Type => AdditionalBonusType.ReplaceItem;

        public override void ApplyBonus(IWorldData worldData) =>
            worldData.ReplaceItems.AddItems(Count);
    }
}