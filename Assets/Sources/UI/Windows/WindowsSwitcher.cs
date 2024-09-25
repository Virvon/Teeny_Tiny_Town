using Assets.Sources.Infrastructure.Factories.UiFactory;
using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs.Windows;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Assets.Sources.UI.Windows
{
    public class WindowsSwitcher
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IUiFactory _uiFactory;
        private readonly Dictionary<WindowType, Window> _windows;

        private Window _currentWindow;

        public WindowsSwitcher(IStaticDataService staticDataService, IUiFactory uiFactory)
        {
            _staticDataService = staticDataService;
            _uiFactory = uiFactory;

            _windows = new();
        }

        public async UniTask CreateWindows()
        {
            foreach(WindowConfig windowConfig in _staticDataService.WindowsConfig.Configs)
            {
                Window window = await _uiFactory.CreateWindow(windowConfig.Type);
                _windows.Add(windowConfig.Type, window);
            }
        }

        public void Switch(WindowType windowTypy)
        {
            _currentWindow?.Hide();
            _currentWindow = _windows[windowTypy];
            _currentWindow.Open();
        }
    }
}
