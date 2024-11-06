using Assets.Sources.Services.StaticDataService.Configs.Building;
using System;

namespace Assets.Sources.Data.World.Currency
{
    [Serializable]
    public class BuildingStoreItemData
    {
        public BuildingType Type;
        public uint BuyingCount;

        public BuildingStoreItemData(BuildingType type)
        {
            Type = type;

            BuyingCount = 0;
        }


        public event Action BuyingCountChanged;

        public void ChangeBuyingCount()
        {
            BuyingCount++;
            BuyingCountChanged?.Invoke();
        }
    }
}
