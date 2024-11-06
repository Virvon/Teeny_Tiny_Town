using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class SettingsData
    {
        public bool IsDarkTheme;
        public bool IsOrthographicCamera;
        public bool IsRotationSnapped;
        public bool IsMusicOn;
        public bool IsSoundsOn;

        public SettingsData()
        {
            IsDarkTheme = false;
            IsOrthographicCamera = false;
            IsMusicOn = true;
            IsSoundsOn = true;
        }

        public event Action ThemeChanged;
        public event Action OrthographicChanged;
        public event Action AudioChanged;

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

        public void ChangeMusicActive(bool value)
        {
            IsMusicOn = value;
            AudioChanged?.Invoke();
        }

        public void ChangeSoundsActive(bool value)
        {
            IsSoundsOn = value;
            AudioChanged?.Invoke();
        }
    }
}
