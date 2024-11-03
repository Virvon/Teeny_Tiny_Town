using System;

namespace Assets.Sources.UI.Windows.Start
{
    public class OrthographicSettingsOption : SettingsOption
    {
        protected override void OnToggleValueChanged(bool value) =>
            PersistentProgressService.Progress.SettingsData.ChangeOrthographic(value);

        protected override bool SetUpToggle() =>
            PersistentProgressService.Progress.SettingsData.IsOrthographicCamera;
    }
}
