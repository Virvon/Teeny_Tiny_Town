namespace Assets.Sources.UI.Windows.Start
{
    public class RotationSnapOption : SettingsOption
    {
        protected override void OnToggleValueChanged(bool value) =>
            PersistentProgressService.Progress.SettingsData.IsRotationSnapped = value;

        protected override bool SetUpToggle() =>
            PersistentProgressService.Progress.SettingsData.IsRotationSnapped;
    }
}
