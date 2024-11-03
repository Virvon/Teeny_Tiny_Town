using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class SettingsData
    {
        public bool IsDarkTheme;
        public bool IsOrthographicCamera;
        public bool IsRotationSnapped;

        public SettingsData()
        {
            IsDarkTheme = false;
            IsOrthographicCamera = false;
        }

        public event Action ThemeChanged;
        public event Action OrthographicChanged;

        public void ChangeTheme(bool isDarkTheme)
        {
            IsDarkTheme = isDarkTheme;
            ThemeChanged?.Invoke();
        }

        public void ChangeOrthographic(bool value)
        {
            IsOrthographicCamera = value;
            OrthographicChanged?.Invoke();
        }
    }
}
