﻿using Assets.Sources.Data;
using Assets.Sources.Gameplay.World.StateMachine.States;
using Assets.Sources.Services.StaticDataService.Configs.World;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Sources.Gameplay.World
{
    public class World : MonoBehaviour
    {
        [SerializeField] private WorldInstaller _worldInstaller;

        public WorldData WorldData { get; private set; }

        public void Init(WorldData worldData)
        {
            WorldData = worldData;
        }

        public void Choose()
        {
            _worldInstaller.WorldStateMachine.Enter<ChangeWorldState>().Forget();
        }

        public class Factory : PlaceholderFactory<string, Vector3, Transform, UniTask<World>>
        {
        }
    }
}
