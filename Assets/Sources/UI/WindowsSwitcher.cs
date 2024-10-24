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

        public void Switch<TWindow>()
            where TWindow : Window
        {
            _currentWindow?.Hide();
            _currentWindow = _windows[typeof(TWindow)];
            _currentWindow.Open();
        }
    }
}
