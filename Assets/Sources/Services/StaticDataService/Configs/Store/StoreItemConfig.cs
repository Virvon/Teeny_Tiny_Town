using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "StoreItemConfig", menuName = "StaticData/Create new store item config", order = 51)]
    public class StoreItemConfig : ScriptableObject
    {
        public StoreItemType Type;
        public uint Cost;
    }
}
