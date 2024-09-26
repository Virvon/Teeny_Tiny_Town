using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.Windows
{
    [CreateAssetMenu(fileName = "WindowsConfig", menuName = "StaticData/Create new windows config", order = 51)]
    public class WindowsConfig : ScriptableObject
    {
        public WindowConfig[] Configs;
    }

}
