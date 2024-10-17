using Assets.Sources.Gameplay.World.Root;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.Gameplay.World
{
    public class CurrencyWorld : World
    {
        public override void EnterBootstrapState()
        {
            WorldInstaller.WorldStateMachine.Enter<CurrencyWorldBootstrapState>().Forget();
        }
    }
}
