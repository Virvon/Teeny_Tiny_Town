using Cysharp.Threading.Tasks;

namespace Assets.Sources.Services.StateMachine
{
    public interface IPayloadState<TPayload> : IExitableState
    {
        UniTask Enter(TPayload payload);
    }
}