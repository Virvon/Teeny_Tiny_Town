using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.World
{
    [CreateAssetMenu(fileName = "SandboxConfig", menuName = "StaticData/WorldConfig/Create new sandbox config", order = 51)]
    public class SandboxConfig : ScriptableObject
    {
        public Vector2Int Size;
        public SandboxGroundConfig[] Grounds;
    }
}
