using Assets.Sources.Data.World;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI.Windows.World.Panels.GainButtons
{
    public class GainButton : ActionHandlerButton
    {
        [SerializeField] private TMP_Text _countValue;

        protected IWorldData WorldData { get; private set; }

        [Inject]
        private void Construct(IWorldData worldData) =>
            WorldData = worldData;

        protected void ChangeCountValue(uint value) =>
            _countValue.text = value.ToString();
    }
}
