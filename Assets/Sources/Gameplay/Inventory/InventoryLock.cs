using Assets.Sources.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.Gameplay.Inventory
{
    public class InventoryLock : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _lockIcon;
        [SerializeField] private Button _button;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService)
        {
            bool isInventoryUnlocked = persistentProgressService.Progress.StoreData.IsInventoryUnlocked;

            _lockIcon.alpha = isInventoryUnlocked ? 0 : 1;
            _button.interactable = isInventoryUnlocked ? true : false;
        }
    }
}
