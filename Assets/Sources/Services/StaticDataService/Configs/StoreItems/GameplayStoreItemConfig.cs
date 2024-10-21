using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;

namespace Assets.Sources.Services.StaticDataService.Configs.Windows
{
    [Serializable]
    public class GameplayStoreItemConfig
    {
        public BuildingType BuildingType;
        public uint Price;
    }
}
