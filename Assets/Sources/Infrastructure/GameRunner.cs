using UnityEngine;
using Zenject;

namespace Assets.Sources.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        private GameBootstrapper.Factory _gameBootstrapperFactory;

        [Inject]
        void Construct(GameBootstrapper.Factory bootstrapperFactory) =>
            _gameBootstrapperFactory = bootstrapperFactory;

        private void Awake()
        {
            var bootstrapper = FindObjectOfType<GameBootstrapper>();

            if (bootstrapper != null)
                return;

            _gameBootstrapperFactory.Create();
        }
    }
}
