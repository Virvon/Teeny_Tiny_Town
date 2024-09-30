using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.UI.Windows
{
    public class Window : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        private void OnDestroy()
        {
            Hide();
        }

        public void Open()
        {
            _canvas.enabled = true;
        }

        public void Hide()
        {
            _canvas.enabled = false;
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, UniTask<Window>>
        {
        }
    }
}
