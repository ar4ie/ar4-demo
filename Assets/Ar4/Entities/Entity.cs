using System.Threading.Tasks;
using Ar4.Base;
using Ar4.Config;
using Ar4.Entities.Abilities;
using Ar4.Entities.Models;
using Ar4.Entities.Players;
using Ar4.Zones;
using Fusion;
using UnityEngine;
using Zenject;

namespace Ar4.Entities
{
    public class Entity : BaseNetworkBehaviour
    {
        [Inject] GameConfig _gameConfig;
        [Inject] Zone _zone;
        public Zone Zone => _zone;

        [HideInInspector][Networked] public Vector2 Position { get; set; }
        [HideInInspector][Networked] public float Rotation { get; set; }
        [SerializeField] public float extend;
        [SerializeField] public float moveSpeed;
        [SerializeField] public float collisionSpeed = 5;

        [HideInInspector][Networked] public AbilityRequest AbilityRequest { get; private set; }
        PropertyReader<AbilityRequest> _abilityRequestPropertyReader;
        [HideInInspector][Networked] public TickTimer ActionTimer { get; set; }

        public bool IsPlayer => Object.InputAuthority != default;
        public bool IsEnemy(Entity e) => e.IsPlayer != IsPlayer;

        [SerializeField] AsyncComponent<EntityModel> asyncModel;
        public EntityModel Model => asyncModel.Component;
        public bool RenderInitialized => Model != null;
        ChangeDetector _renderChangeDetector;

        public override async void Spawned()
        {
            base.Spawned();
            _zone.OnEntitySpawned(this);
            await CreateModel();
            _renderChangeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
            _abilityRequestPropertyReader = GetPropertyReader<AbilityRequest>(nameof(AbilityRequest));
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            if (RenderInitialized)
                Model.OnDespawn();
            DestroyModel();
            _zone.OnEntityDespawned(this);
            base.Despawned(runner, hasState);
        }

        public async Task CreateModel()
        {
            if (!Runner.IsPlayer)
                return;
            await asyncModel.Load(transform);
        }

        public void DestroyModel()
        {
            asyncModel.Unload();
        }

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();
            if (Object.IsProxy)
                return;
            if (AbilityRequest.TickUpdate(AbilityRequest, this, _gameConfig))
                RequestAbility(default);
            PhysicsUtil.Collide(this, _zone.Entities.Values, collisionSpeed);
        }

        public void RequestAbility(AbilityRequest abilityRequest)
        {
            AbilityRequest = AbilityRequest.TickRequest(abilityRequest, AbilityRequest, this, _gameConfig);
        }

        public override void Render()
        {
            base.Render();
            if (!Runner.IsPlayer || !RenderInitialized)
                return;

            // todo use change detector?
            var interpolator = new NetworkBehaviourBufferInterpolator(this);
            var position = interpolator.Vector2(nameof(Position));
            transform.position = new Vector3(position.x, _zone.terrain.GetHeight(position), position.y);
            var rotation = interpolator.Float(nameof(Rotation));
            transform.rotation = Quaternion.AngleAxis(rotation * Mathf.Rad2Deg, Vector3.up);

            foreach (var change in _renderChangeDetector.DetectChanges(this, out var prevBuffer, out var currBuffer))
            {
                switch (change)
                {
                    case nameof(AbilityRequest):
                        AbilityRequest.RenderRequest(_abilityRequestPropertyReader.Read(currBuffer), _abilityRequestPropertyReader.Read(prevBuffer), this, _gameConfig);
                        break;
                }
            }
        }

        public void Hit(Entity source)
        {
            if (!Runner.IsServer)
                return;
            if (!source.IsEnemy(this))
                return;
            Despawn();
            if (source.IsPlayer)
                source.GetComponent<Player>().ChangeScore(+1);
        }

        public void Despawn()
        {
            _zone.DespawnEntity(Runner, this);
        }
    }
}