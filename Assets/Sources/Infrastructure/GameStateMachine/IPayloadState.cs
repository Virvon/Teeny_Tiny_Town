using Cysharp.Threading.Tasks;

namespace Assets.Sources.Infrastructure.GameStateMachine
{
    public interface IPayloadState<TPayload> : IExitableState
    {
        UniTask Enter(TPayload payload);
    }
}