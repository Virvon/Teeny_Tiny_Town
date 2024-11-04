using Zenject;

namespace Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler
{
    public class ActionHandlerStatesFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly DiContainer _container;

        public ActionHandlerStatesFactory(IInstantiator instantiator, DiContainer container)
        {
            _instantiator = instantiator;
            _container = container;
        }

        public TState CreateHandlerState<TState>()
            where TState : ActionHandlerState
        {
            TState state = _instantiator.Instantiate<TState>();
            _container.BindInterfacesAndSelfTo<TState>().FromInstance(state).AsSingle();

            return state;
        }
    }
}