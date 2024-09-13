using Cysharp.Threading.Tasks;

namespace Assets.Sources.Services.StateMachine
{
    public interface IExitableState
    {
        UniTask Exit();
    }
}