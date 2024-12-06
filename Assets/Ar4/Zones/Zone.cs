using System;
using System.Collections.Generic;
using Ar4.Base;
using Ar4.Base.UI;
using Ar4.Entities;
using Ar4.Entities.Players;
using Ar4.Terrains;
using Ar4.Zones.UI;
using Fusion;
using UnityEngine;
using Zenject;

namespace Ar4.Zones
{
    public class Zone : BaseNetworkBehaviour
    {
        [Inject] NetworkEvents _networkEvents;
        [Inject] ScreensController _screensController;
        [SerializeField] ZoneScreen zoneScreenPrefab;
        
        [SerializeField] Player playerPrefab;
        Dictionary<PlayerRef, Player> _players = new ();
        public IReadOnlyDictionary<PlayerRef, Player> Players => _players;
        public Action<Player> PlayerSpawned;
        public Action<Player> PlayerDespawned;

        Dictionary<NetworkBehaviourId, Entity> _entities = new ();
        public IReadOnlyDictionary<NetworkBehaviourId, Entity> Entities => _entities;

        [SerializeField] public TerrainController terrain;

        void Awake()
        {
            FindObjectOfType<SceneContext>().Container.BindInstance(this);
        }

        public override async void Spawned()
        {
            base.Spawned();
            if (Runner.IsServer)
            {
                _networkEvents.PlayerJoined.AddListener(OnPlayerJoined);
                _networkEvents.PlayerLeft.AddListener(OnPlayerLeft);
                if (Runner.LocalPlayer != default)
                    OnPlayerJoined(Runner, Runner.LocalPlayer);
            }
            if (Runner.IsPlayer)
                _screensController.OpenScreen(zoneScreenPrefab, this);

            await terrain.CreateModel();
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            terrain.DestroyModel();

            if (Runner.IsServer)
            {
                _networkEvents.PlayerJoined.RemoveListener(OnPlayerJoined);
                _networkEvents.PlayerLeft.RemoveListener(OnPlayerLeft);
            }
            base.Despawned(runner, hasState);
        }

        void OnPlayerJoined(NetworkRunner runner, PlayerRef playerRef)
        {
            SpawnEntity(runner, playerPrefab.Entity, default, default, playerRef);
        }

        public void OnPlayerSpawned(Player player)
        {
            _players.Add(player.Object.InputAuthority, player);
            PlayerSpawned?.Invoke(player);
        }

        void OnPlayerLeft(NetworkRunner runner, PlayerRef playerRef)
        {
            DespawnEntity(runner, _players[playerRef].Entity);
        }

        public void OnPlayerDespawned(Player player)
        {
            _players.Remove(player.Object.InputAuthority);
            PlayerDespawned?.Invoke(player);
        }

        public void SpawnEntity(NetworkRunner runner, Entity entityPrefab, Vector2 position, float rotation, PlayerRef? inputAuthority = null)
        {
            runner.Spawn(entityPrefab, Vector3.zero, Quaternion.identity, onBeforeSpawned: (_, obj) => {
                var e = obj.GetComponent<Entity>();
                e.Position = position;
                e.Rotation = rotation;
            }, inputAuthority: inputAuthority);
        }

        public void OnEntitySpawned(Entity entity)
        {
            _entities.Add(entity.Id, entity);
        }

        public void DespawnEntity(NetworkRunner runner, Entity entity)
        {
            runner.Despawn(entity.Object);
        }

        public void OnEntityDespawned(Entity entity)
        {
            _entities.Remove(entity.Id);
        }
    }
}