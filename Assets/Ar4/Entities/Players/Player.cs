using System;
using Ar4.Base;
using Ar4.Render;
using Ar4.Zones;
using Fusion;
using UnityEngine;
using Zenject;

namespace Ar4.Entities.Players
{
    public class Player : BaseNetworkBehaviour
    {
        [Inject] Zone _zone;
        [Inject] RenderController _renderController;

        [SerializeField] Entity entity;
        public Entity Entity => entity;

        [HideInInspector][Networked] public int Score { get; private set; }
        public event Action RenderScoreChanged;
        ChangeDetector _renderChangeDetector;

        public override void Spawned()
        {
            base.Spawned();
            _zone.OnPlayerSpawned(this);
            if (Object.HasInputAuthority)
                _renderController.SetCameraTarget(gameObject);
            _renderChangeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            if (Object.HasInputAuthority)
                _renderController.SetCameraTarget(null);
            _zone.OnPlayerDespawned(this);
            base.Despawned(runner, hasState);
        }

        public override void Render()
        {
            base.Render();
            if (!Runner.IsPlayer || !entity.RenderInitialized)
                return;

            foreach (var change in _renderChangeDetector.DetectChanges(this, out var prevBuffer, out var currBuffer))
            {
                switch (change)
                {
                    case nameof(Score):
                        RenderScoreChanged?.Invoke();
                        break;
                }
            }
        }

        public void ChangeScore(int change)
        {
            Score += change;
        }
    }
}