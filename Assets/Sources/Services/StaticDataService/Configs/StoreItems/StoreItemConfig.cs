using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;

namespace Assets.Sources.Services.StaticDataService.Configs.Windows
{
    [Serializable]
    public class StoreItemConfig
    {
        public BuildingType BuildingType;
        public uint Cost;
    }
}
