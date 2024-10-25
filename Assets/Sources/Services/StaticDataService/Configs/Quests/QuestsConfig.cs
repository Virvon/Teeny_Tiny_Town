using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Sources.Services.StaticDataService.Configs.Quests
{
    [CreateAssetMenu(fileName = "QuestsConfig", menuName = "StaticData/Create new quests config", order = 51)]
    public class QuestsConfig : ScriptableObject
    {
        public QuestConfig[] Configs;
        public string[] StartQuestsId;
        public AssetReferenceGameObject QuestPanelAssetReference;

        public QuestConfig GetQuest(string id) =>
            Configs.First(config => config.Id == id);
    }
}