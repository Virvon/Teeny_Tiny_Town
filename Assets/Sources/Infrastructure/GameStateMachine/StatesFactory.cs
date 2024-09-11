using Zenject;

namespace Assets.Sources.Infrastructure.GameStateMachine
{
    public class StatesFactory
    {
        private IInstantiator _instantiator;

        public StatesFactory(IInstantiator instantiator) =>
            _instantiator = instantiator;

        public TState Create<TState>()
            where TState : IExitableState =>
            _instantiator.Instantiate<TState>();
    }
}