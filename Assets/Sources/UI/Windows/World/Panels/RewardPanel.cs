using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.World.Panels
{
    public class RewardPanel : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        public void Init(Sprite icon) =>
            _icon.sprite = icon;

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Transform, UniTask<RewardPanel>>
        {
        }
    }
}
