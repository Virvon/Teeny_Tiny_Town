using Assets.Sources.Data;
using UnityEngine;

namespace Assets.Sources.Gameplay.World
{
    public class ExpandingWorldDataInstaller : WorldDataInstaller
    {
        protected override void BindWorldData()
        {
            if (WorldData is not ExpandingWorldData)
                Debug.LogError(nameof(WorldData) + " is not " + typeof(ExpandingWorldData));

            Container.BindInterfacesAndSelfTo<WorldData>().FromInstance(WorldData as ExpandingWorldData).AsSingle();
        }
    }
}
