using Ar4.Base;
using Ar4.Entities;
using Fusion;
using UnityEngine;
using Zenject;

namespace Ar4.Zones
{
    public class EntitiesSpawner : BaseNetworkBehaviour
    {
        [Inject] Zone _zone;
        [SerializeField] Entity[] monsterPrefabs;
        [SerializeField] float spawnPeriod;
        TickTimer _spawnTimer;

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();
            if (!Runner.IsServer || !_zone.terrain.Initialized)
                return;
            if (!_spawnTimer.ExpiredOrNotRunning(Runner))
                return;
            var monsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
            _zone.SpawnEntity(Runner, monsterPrefab, _zone.terrain.GetRandomPosition(), 2 * Mathf.PI * Random.value);
            _spawnTimer = TickTimer.CreateFromSeconds(Runner, spawnPeriod);
        }
    }
}