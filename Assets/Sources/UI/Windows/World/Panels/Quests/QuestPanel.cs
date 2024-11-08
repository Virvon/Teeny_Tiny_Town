using System;
using Assets.Sources.Data;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Quests;
using Cysharp.Threading.Tasks;
using MPUIKIT;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.World.Panels
{
    public class QuestPanel : MonoBehaviour
    {
        private const string CompleteIfno = "Получить";

        [SerializeField] private TMP_Text _info;
        [SerializeField] private TMP_Text _progress;
        [SerializeField] private TMP_Text _rewardValue;
        [SerializeField] private MPImage _fill;
        [SerializeField] private Button _button;

        private IPersistentProgressService _persistentProgressService;
        private IStaticDataService _staticDataService;

        private QuestData _data;
        private QuestConfig _config;

        public event Action<QuestPanel> Clicked;

        public string Id { get; private set; }

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService, IStaticDataService staticDataService)
        {
            _persistentProgressService = persistentProgressService;
            _staticDataService = staticDataService;
        }

        private void OnDestroy()
        {
            _data.Progressed -= ChangeProgressbar;
            _data.Completed -= Complete;
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        public void Init(string id)
        {
            Id = id;

            _config = _staticDataService.QuestsConfig.GetQuest(id);
            _data = _persistentProgressService.Progress.GetQuest(id);

            _info.text = _config.Info;
            _rewardValue.text = _config.Reward.ToString();

            if (_data.IsCompleted)
                Complete();
            else
                ChangeProgressbar();

            _data.Progressed += ChangeProgressbar;
            _data.Completed += Complete;
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            if (_data.IsCompleted == false)
                Debug.LogError("quest is not completed");

            Clicked?.Invoke(this);
        }

        private void ChangeProgressbar()
        {
            _progress.text = $"{_data.Progress}/{_config.TargetCount}";
            _fill.fillAmount = (float)_data.Progress / _config.TargetCount;
        }

        private void Complete()
        {
            _progress.text = CompleteIfno;
            _fill.fillAmount = 1;
            _button.interactable = true;
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Transform, UniTask<QuestPanel>>
        {
        }
    }
}
