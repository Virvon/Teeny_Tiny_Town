using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.Building
{
    [CreateAssetMenu(fileName = "ChestConfig", menuName = "StaticData/Building/Create new chest config", order = 51)]
    public class ChestConfig : BuildingConfig
    {
        public uint Reward;
    }
}
