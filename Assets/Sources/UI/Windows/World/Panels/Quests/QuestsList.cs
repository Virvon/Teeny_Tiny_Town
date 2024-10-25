﻿using Assets.Sources.Data;
using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Quests;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
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

            _questPanels = new();

            foreach (QuestData questData in _persistentPorgressService.Progress.Quests)
                await CreateQuestPanel(questData.Id);
        }

        private void OnDestroy()
        {
            foreach (QuestPanel questPanel in _questPanels)
                questPanel.Clicked -= OnQuestPanelClicked;
        }

        private async void OnQuestPanelClicked(QuestPanel questPanel)
        {
            QuestConfig questConfig = _staticDataService.QuestsConfig.GetQuest(questPanel.Id);

            _persistentPorgressService.Progress.Wallet.Give(questConfig.Reward);

            List<QuestData> questDatas = _persistentPorgressService.Progress.Quests;
            questDatas.Remove(questDatas.First(questData => questData.Id == questPanel.Id));

            _questPanels.Remove(questPanel);
            questPanel.Clicked -= OnQuestPanelClicked;
            Destroy(questPanel.gameObject);

            await CreateQuest();
        }

        private async UniTask CreateQuest()
        {
            bool isUniqueQuest = false;
            string questId = string.Empty;

            while(isUniqueQuest == false)
            {
                QuestConfig[] questConfigs = _staticDataService.QuestsConfig.Configs;
                questId = questConfigs[Random.Range(0, questConfigs.Length + 1)].Id;

                if (_persistentPorgressService.Progress.Quests.Any(questData => questData.Id == questId) == false)
                    isUniqueQuest = true;
            }

            _persistentPorgressService.Progress.Quests.Add(new QuestData(questId));
            await CreateQuestPanel(questId);
        }

        private async UniTask CreateQuestPanel(string id)
        {
            QuestPanel questPanel = await _uiFactory.CreateQuestPanel(id, transform);

            _questPanels.Add(questPanel);
            questPanel.Clicked += OnQuestPanelClicked;
        }
    }
}
