using System.Collections.Generic;
using System.Linq;
using Assets.Sources.Data;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Quests;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Windows.World.Panels
{
    public class QuestsList : MonoBehaviour
    {
        private IPersistentProgressService _persistentPorgressService;
        private IUiFactory _uiFactory;
        private IStaticDataService _staticDataService;

        private List<QuestPanel> _questPanels;

        [Inject]
        private async void Construct(IPersistentProgressService persistentProgressService, IUiFactory uiFactory, IStaticDataService staticDataService)
        {
            _persistentPorgressService = persistentProgressService;
            _uiFactory = uiFactory;
            _staticDataService = staticDataService;

            _questPanels = new ();

            foreach (QuestData questData in _persistentPorgressService.Progress.Quests)
                await CreateQuestPanel(questData.Id);

            _persistentPorgressService.Progress.QuestChanged += OnQuestChanged;
        }

        private void OnDestroy()
        {
            foreach (QuestPanel questPanel in _questPanels)
                questPanel.Clicked -= OnQuestPanelClicked;

            _persistentPorgressService.Progress.QuestChanged -= OnQuestChanged;
        }

        private void OnQuestPanelClicked(QuestPanel questPanel)
        {
            QuestConfig questConfig = _staticDataService.QuestsConfig.GetQuest(questPanel.Id);

            _persistentPorgressService.Progress.Wallet.Give(questConfig.Reward);

            bool isUniqueQuest = false;
            string questId = string.Empty;

            while (isUniqueQuest == false)
            {
                QuestConfig[] questConfigs = _staticDataService.QuestsConfig.Configs;
                questId = questConfigs[Random.Range(0, questConfigs.Length + 1)].Id;

                if (_persistentPorgressService.Progress.Quests.Any(questData => questData.Id == questId) == false)
                    isUniqueQuest = true;
            }

            _persistentPorgressService.Progress.ChangeQuest(questPanel.Id, new QuestData(questId));
        }

        private async void OnQuestChanged(string changedQuestId, string newQuestId)
        {
            QuestPanel questPanel = _questPanels.First(panel => panel.Id == changedQuestId);

            _questPanels.Remove(questPanel);
            questPanel.Clicked -= OnQuestPanelClicked;
            Destroy(questPanel.gameObject);

            await CreateQuestPanel(newQuestId);
        }

        private async UniTask CreateQuestPanel(string id)
        {
            QuestPanel questPanel = await _uiFactory.CreateQuestPanel(id, transform);

            _questPanels.Add(questPanel);
            questPanel.Clicked += OnQuestPanelClicked;
        }
    }
}
