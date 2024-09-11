using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Sources.WorldGenerator
{
    public class Soil : MonoBehaviour, IPointerClickHandler, IPointerMoveHandler
    {
        public Transform BuildingPoint;

        public event Action<Soil> PointerMoved;

        public void OnPointerClick(PointerEventData eventData)
        {

        }

        public void OnPointerMove(PointerEventData eventData)
        {
            PointerMoved?.Invoke(this);
        }
    }
}
