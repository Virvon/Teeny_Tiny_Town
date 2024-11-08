using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.Birds
{
    public class Bird : MonoBehaviour
    {
        [SerializeField] private Vector3[] _points;
        [SerializeField] private float _speed;

        private Tween _mover;

        private void Start()
        {
            transform.position = GetPoint();

            MoveToPoint(GetPoint());
        }

        private void OnDestroy() =>
            _mover?.Kill();

        private void MoveToPoint(Vector3 point)
        {
            Vector3 lookDirection = (point - transform.position).normalized;

            if (lookDirection != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(lookDirection);

            _mover?.Kill();

            _mover = transform.DOMove(point, _speed).SetSpeedBased().OnComplete(() =>
            {
                MoveToPoint(GetPoint());
            });
        }

        private Vector3 GetPoint() =>
            _points[Random.Range(0, _points.Length)];

        public class Factory : PlaceholderFactory<string, UniTask<Bird>>
        {
        }
    }
}
