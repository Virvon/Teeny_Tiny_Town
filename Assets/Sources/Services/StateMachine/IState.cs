using Cysharp.Threading.Tasks;

namespace Assets.Sources.Services.StateMachine
{
    public interface IState : IExitableState
    {
        UniTask Enter();
    }
}