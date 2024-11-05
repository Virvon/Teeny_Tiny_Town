using Assets.Sources.Services.Input;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Sources.UI
{
    public class Test2 : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private IInputService _inputService;
        

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;

            _inputService.Rotated += x => _text.text = x.ToString();
        }
    }
}
