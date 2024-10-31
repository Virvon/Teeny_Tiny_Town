using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class BuildingData
    {
        public BuildingType Type;
        public bool isUnlocked;

        public BuildingData(BuildingType type)
        {
            Type = type;

            isUnlocked = false;
        }
    }
}
