using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI.Panels
{
    public class Blur : MonoBehaviour
    {
        private const string BlurMaterial = "_Blur";
        private const float MaxBlur = 0.015f;
        private const float MinBlur = 0;

        [SerializeField] private Image _blur;

        public void Show(float duration) =>
            ChangeBlured(MaxBlur, duration);

        public void Hide(float duration) =>
            ChangeBlured(MinBlur, duration);

        private void ChangeBlured(float targetValue, float duration) =>
            _blur.material.DOFloat(targetValue, BlurMaterial, duration);
    }
}
