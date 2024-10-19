using Assets.Sources.Gameplay.Cameras;
using Assets.Sources.Gameplay.World;
using Assets.Sources.Services.StateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Gameplay.StateMachine.States
{
    public class GameplayLoopState : IState
    {
        private readonly WorldsList _worldsList;
        private readonly GameplayCamera _camera;

        public GameplayLoopState(WorldsList worldsList, GameplayCamera camera)
        {
            _worldsList = worldsList;
            _camera = camera;
        }

        public UniTask Enter()
        {
            _camera.MoveTo(new Vector3(55.1f, 78.8f, -55.1f));
            _worldsList.StartCurrentWorld();
            return default;
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
