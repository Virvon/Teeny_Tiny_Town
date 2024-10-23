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

        public GameplayLoopState(WorldsList worldsList)
        {
            _worldsList = worldsList;
        }

        public UniTask Enter()
        {
            _worldsList.StartCurrentWorld();
            return default;
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}
