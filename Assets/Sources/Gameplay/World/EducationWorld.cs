using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World
{
    public class EducationWorld : World
    {
        public class EducationFactory : PlaceholderFactory<string, Vector3, Transform, UniTask<EducationWorld>>
        {
        }
    }
}
