using Cysharp.Threading.Tasks;

namespace Assets.Sources.Infrastructure.GameStateMachine
{
    public interface IState : IExitableState
    {
        UniTask Enter();
    }
}