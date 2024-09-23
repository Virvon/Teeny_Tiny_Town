using Assets.Sources.Data;

namespace Assets.Sources.Services.SaveLoadProgress
{
    public interface ISaveLoadService
    {
        void SaveProgress();

        PlayerProgress LoadProgress();
    }
}