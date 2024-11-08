using System.Linq;
using Assets.Sources.Data.World;
using Assets.Sources.Data.World.Currency;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.World
{
    [CreateAssetMenu(fileName = "WorldConfig", menuName = "StaticData/WorldConfig/Create new expanding world config", order = 51)]
    public class ExpandingWorldConfig : CurrencyWorldConfig
    {
        public Vector2Int StartSize;
        public ExpandConfig[] ExpandConfigs;

        public bool ContainsExpand(BuildingType type, out ExpandConfig expandConfig)
        {
            expandConfig = ExpandConfigs.FirstOrDefault(config => config.BuidldingType == type);

            return expandConfig != null;
        }

        public override WorldData GetWorldData(uint[] goals, IStaticDataService staticDataService) =>
            new CurrencyWorldData(Id, TilesDatas, NextBuildingTypeForCreation, StartingAvailableBuildingTypes.ToList(), StartSize, StartStoreList, goals, GetGainStoreItemsList(staticDataService), IsUnlockedOnStart, StartWorldWalletValue);
    }
}