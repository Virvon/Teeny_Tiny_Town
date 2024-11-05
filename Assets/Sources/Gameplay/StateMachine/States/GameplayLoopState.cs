using Assets.Sources.Gameplay.World;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.StateMachine.States
{
    public class GameplayLoopState : IPayloadState<bool>
    {
        private readonly WorldsList _worldsList;

        public GameplayLoopState(WorldsList worldsList) =>
            _worldsList = worldsList;

        public async UniTask Enter(bool startCurrentWorld)
        {
            if (startCurrentWorld)
                await _worldsList.StartCurrentWorld();
            else
                await _worldsList.StartLastPlayedWorld();
        }

        public UniTask Exit() =>
            default;
    }
}
