namespace Assets.Sources.UI.Windows.Start
{
    public class SoundsOption : SettingsOption
    {
        protected override void OnToggleValueChanged(bool value) =>
            PersistentProgressService.Progress.SettingsData.ChangeSoundsActive(value);

        protected override bool SetUpToggle() =>
            PersistentProgressService.Progress.SettingsData.IsSoundsOn;
    }
}
