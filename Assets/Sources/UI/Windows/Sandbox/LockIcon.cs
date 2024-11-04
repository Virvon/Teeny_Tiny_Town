using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Windows.Sandbox
{
    public class LockIcon : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<string, Transform, UniTask<LockIcon>>
        {
        }
    }
}
