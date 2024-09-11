using Cysharp.Threading.Tasks;

namespace Assets.Sources.Infrastructure.GameStateMachine
{
    public interface IExitableState
    {
        UniTask Exit();
    }
}