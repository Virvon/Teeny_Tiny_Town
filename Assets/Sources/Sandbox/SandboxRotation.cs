using Assets.Sources.Gameplay.World;

namespace Assets.Sources.Sandbox
{
    public class SandboxRotation : IWorldRotation
    {
        private SandboxWorld _sandboxWorld;

        public float RotationDegrees => _sandboxWorld.Rotation;

        public void Init(SandboxWorld sandboxWorld) =>
            _sandboxWorld = sandboxWorld;
    }
}
