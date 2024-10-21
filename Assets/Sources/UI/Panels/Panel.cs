using Assets.Sources.Services.StaticDataService;
using Assets.Sources.Services.StaticDataService.Configs;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Panels
{
    public abstract class Panel : MonoBehaviour
    {
        [Inject]
        private void Construct(IStaticDataService staticDataService)
        {
            AnimationsConfig = staticDataService.AnimationsConfig;
        }

        protected AnimationsConfig AnimationsConfig { get; private set; }

        public abstract void Open();
        public virtual void Hide()
        {
        }
    }
}
