using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.UI
{
    public class Window : MonoBehaviour, IWindow
    {
        [SerializeField] private Canvas _canvas;

        protected Canvas Canvas => _canvas;

        public virtual void Open()
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

        public void Destroy() =>
            Destroy(gameObject);
    }
}
