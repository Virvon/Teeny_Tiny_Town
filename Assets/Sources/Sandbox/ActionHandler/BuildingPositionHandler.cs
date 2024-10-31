using Assets.Sources.Gameplay.World.RepresentationOfWorld.ActionHandler;
using Assets.Sources.Gameplay.World.RepresentationOfWorld.Markers;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Assets.Sources.Sandbox.ActionHandler
{
    public class BuildingPositionHandler : ActionHandlerState
    {
        public BuildingPositionHandler(SelectFrame selectFrame, LayerMask layerMask)
            : base(selectFrame, layerMask)
        {
        }

        public override UniTask Enter()
        {
            throw new NotImplementedException();
        }

        public override UniTask Exit()
        {
            throw new NotImplementedException();
        }

        public override void OnHandleMoved(Vector2 handlePosition)
        {
            throw new NotImplementedException();
        }

        public override void OnPressed(Vector2 handlePosition)
        {
            throw new NotImplementedException();
        }
    }
}
