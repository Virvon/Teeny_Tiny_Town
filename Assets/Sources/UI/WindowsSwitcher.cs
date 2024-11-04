using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class WindowsSwitcher
    {
        private readonly Dictionary<Type, Window> _windows;

        private Window _currentWindow;

        public WindowsSwitcher()
        {
            _windows = new();
        }

        public async UniTask RegisterWindow<TWindow>(WindowType windowType, IUiFactory uiFactory)
            where TWindow : Window
        {
            Window window = await uiFactory.CreateWindow(windowType);

            if(window is not TWindow)
                Debug.LogError($"{nameof(window)} is not {typeof(TWindow)}");

            _windows.Add(typeof(TWindow), window);
        }

        public void Switch<TWindow>(string str)
            where TWindow : Window
        {
            if (_currentWindow != null)
                _currentWindow.Hide(callback: () => OpenWindow<TWindow>());
            else
                OpenWindow<TWindow>();
        }

        public void Remove<TWindow>()
            where TWindow : Window
        {
            Window window = _windows[typeof(TWindow)];

            if (_currentWindow == window)
            {
                _currentWindow.Hide(callback: () =>
                {
                    Remove<TWindow>(window);
                    _currentWindow = null;
                });
            }
            else
            {
                Remove<TWindow>(window);
            }
        }

        private void Remove<TWindow>(Window window) where TWindow : Window
        {
            window.Destroy();
            _windows.Remove(typeof(TWindow));
        }

        private void OpenWindow<TWindow>()
            where TWindow : Window
        {
            _currentWindow = _windows[typeof(TWindow)];
            _currentWindow.Open();
        }
    }
}
