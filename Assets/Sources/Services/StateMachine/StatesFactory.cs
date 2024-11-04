using Zenject;

namespace Assets.Sources.Services.StateMachine
{
    public class StatesFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly DiContainer _container;

        public StatesFactory(IInstantiator instantiator, DiContainer container)
        {
            _instantiator = instantiator;
            _container = container;
        }

        public TState Create<TState>()
            where TState : IExitableState
        {
            TState state = _instantiator.Instantiate<TState>();
            _container.BindInterfacesTo<TState>().FromInstance(state);

            return state;
        }
    }
}