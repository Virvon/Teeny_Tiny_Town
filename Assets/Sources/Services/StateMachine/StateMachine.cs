using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Services.StateMachine
{
    public abstract class StateMachine : IStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;

        private IExitableState _currentState;

        public StateMachine() =>
            _states = new ();

        public async UniTask Enter<TState>()
            where TState : class, IState
        {
            TState state = await ChangeState<TState>();
            await state.Enter();
        }

        public async UniTask Enter<TState, TPayload>(TPayload payload)
            where TState : class, IPayloadState<TPayload>
        {
            TState state = await ChangeState<TState>();
            await state.Enter(payload);
        }

        public void RegisterState<TState>(TState state)
            where TState : IExitableState =>
            _states.Add(typeof(TState), state);

        private TState GetState<TState>()
            where TState : class, IExitableState =>
                _states[typeof(TState)] as TState;

        private async UniTask<TState> ChangeState<TState>()
            where TState : class, IExitableState
        {
            if (_currentState != null)
                await _currentState.Exit();

            TState state = GetState<TState>();

            _currentState = state;

            return state;
        }
    }
}
