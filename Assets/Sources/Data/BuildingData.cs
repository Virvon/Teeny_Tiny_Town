using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class BuildingData
    {
        public BuildingType Type;
        public uint Count;

        public BuildingData(BuildingType type)
        {
            Type = type;

            Count = 0;
        }

        public bool IsUnlocked => Count > 0;
    }
}
