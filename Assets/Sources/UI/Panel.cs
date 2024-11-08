using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI
{
    public abstract class Panel : MonoBehaviour
    {
        protected AnimationsConfig AnimationsConfig { get; private set; }

        [Inject]
        private void Construct(IStaticDataService staticDataService)
        {
            AnimationsConfig = staticDataService.AnimationsConfig;
        }

        public abstract void Open();
        public virtual void Hide()
        {
        }
    }
}
