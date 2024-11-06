namespace Assets.Sources.UI.Windows.Start
{
    public class MusicOption : SettingsOption
    {
        protected override void OnToggleValueChanged(bool value) =>
            PersistentProgressService.Progress.SettingsData.ChangeMusicActive(value);

        protected override bool SetUpToggle() =>
            PersistentProgressService.Progress.SettingsData.IsMusicOn;
    }
}
