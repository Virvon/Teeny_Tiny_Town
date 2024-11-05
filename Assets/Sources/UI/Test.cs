using Assets.Sources.Services.Input;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private IInputService _inputService;
        

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;

            _inputService.Pressed += _ => _text.text = "tous ended";
            _inputService.HandlePressedMoveStarted += _ => _text.text = "pressed move started";
            _inputService.HandlePressedMovePerformed += _ => _text.text = "pressed move performed";
            _inputService.HandleMoved += _ => _text.text = "handle moved";
        }
    }
}
