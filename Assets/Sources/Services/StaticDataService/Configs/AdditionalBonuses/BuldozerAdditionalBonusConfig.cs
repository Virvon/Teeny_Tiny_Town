﻿using Assets.Sources.Data.World;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.AdditionalBonuses
{
    [CreateAssetMenu(fileName = "BuldozerAdditionalBonusConfig", menuName = "StaticData/AdditionalBonus/Create new buldozer additional bonus config", order = 51)]
    public class BuldozerAdditionalBonusConfig : AdditionalBonusConfig
    {
        public override AdditionalBonusType Type => AdditionalBonusType.Buldozer;

        public override void ApplyBonus(IWorldData worldData) =>
            worldData.BulldozerItems.AddItems(Count);
    }
}