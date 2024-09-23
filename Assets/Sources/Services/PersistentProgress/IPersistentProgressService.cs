using Assets.Sources.Data;

namespace Assets.Sources.Services.PersistentProgress
{
    public interface IPersistentProgressService
    {
        public PlayerProgress Progress { get; set; }
    }
}