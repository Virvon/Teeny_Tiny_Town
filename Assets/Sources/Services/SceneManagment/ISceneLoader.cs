using Cysharp.Threading.Tasks;

namespace Assets.Sources.Services.SceneManagment
{
    public interface ISceneLoader
    {
        UniTask Load(string scene);
    }
}