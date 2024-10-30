using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World.Windows
{
    public interface IWorldWindows
    {
        bool IsRegistered { get; }

        UniTask Register();
        void Remove();
    }
}