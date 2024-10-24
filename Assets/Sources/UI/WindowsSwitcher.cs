using Assets.Sources.Services.StaticDataService.Configs.Windows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Sources.UI
{
    public class WindowsSwitcher
    {
        private readonly Dictionary<WindowType, Window> _windows;

        private Window _currentWindow;

        public WindowsSwitcher()
        {
            _windows = new();
        }

        public void RegisterWindow(WindowType type, Window window)
        {
            _windows.Add(type, window);
        }

        public void Switch(WindowType windowTypy)
        {
            _currentWindow?.Hide();
            _currentWindow = _windows[windowTypy];
            _currentWindow.Open();
        }

        public bool Contains(WindowType windowType) =>
            _windows.Keys.Contains(windowType);

        public void Remove(WindowType gameplayWindow)
        {
            _windows.Remove(gameplayWindow);
        }
    }
}
