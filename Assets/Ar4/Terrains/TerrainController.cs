using System.Threading.Tasks;
using Ar4.Base;
using UnityEngine;

namespace Ar4.Terrains
{
    public class TerrainController : BaseNetworkBehaviour
    {
        [SerializeField] AsyncComponent<Terrain> asyncTerrain;
        Terrain Terrain => asyncTerrain.Component;
        public bool Initialized => Terrain != null;

        public async Task CreateModel()
        {
            // todo if (!Runner.IsPlayer) load data, skip visuals
            await asyncTerrain.Load();
        }

        public void DestroyModel()
        {
            asyncTerrain.Unload();
        }

        public Vector2 GetRandomPosition()
        {
            var bounds = Terrain.terrainData.bounds;
            var position = Terrain.GetPosition() + bounds.center;
            return new Vector2(
                position.x + bounds.extents.x * (2 * Random.value - 1),
                position.z + bounds.extents.z * (2 * Random.value - 1)
                );
        }

        public float GetHeight(Vector2 position)
        {
            return Terrain != null ? Terrain.SampleHeight(new Vector3(position.x, 0, position.y)) : 0;
        }
    }
}