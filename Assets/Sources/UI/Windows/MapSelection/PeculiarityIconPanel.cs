using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Sources.UI.Windows.MapSelection
{
    public class PeculiarityIconPanel : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        public void Init(Sprite icon) =>
            _icon.sprite = icon;

        public class Factory : PlaceholderFactory<string, Transform, UniTask<PeculiarityIconPanel>>
        {
        }
    }
}