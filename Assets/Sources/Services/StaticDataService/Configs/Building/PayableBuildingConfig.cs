using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "PayableBuildingConfig", menuName = "StaticData/Building/Create new payable building config", order = 51)]
    public class PayableBuildingConfig : BuildingConfig
    {
        public uint Payment;
    }
}
