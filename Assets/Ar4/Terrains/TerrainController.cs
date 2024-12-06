using System.Threading.Tasks;
using Ar4.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Ar4.Terrains
{
    public class TerrainController : BaseNetworkBehaviour
    {
        [SerializeField] AssetReference terrainPrefab;
        Terrain _terrain;
        public bool Initialized => _terrain != null;

        public async Task CreateModel()
        {
            // todo if (!Runner.IsPlayer) load data, skip visuals
            _terrain = (await terrainPrefab.InstantiateAsync().Task).GetComponent<Terrain>();
        }

        public void DestroyModel()
        {
            if (_terrain != default)
            {
                Addressables.ReleaseInstance(_terrain.gameObject);
                Destroy(_terrain);
                _terrain = default;
            }
        }

        public Vector2 GetRandomPosition()
        {
            var bounds = _terrain.terrainData.bounds;
            var position = _terrain.GetPosition() + bounds.center;
            return new Vector2(
                position.x + bounds.extents.x * (2 * Random.value - 1),
                position.z + bounds.extents.z * (2 * Random.value - 1)
                );
        }

        public float GetHeight(Vector2 position)
        {
            return _terrain != null ? _terrain.SampleHeight(new Vector3(position.x, 0, position.y)) : 0;
        }
    }
}