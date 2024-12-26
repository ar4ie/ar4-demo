using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Ar4.Base
{
    [Serializable]
    public class AsyncComponent<T> where T : Component
    {
        [SerializeField] AssetReference prefab;
        AsyncOperationHandle<GameObject> _handle;
        public T Component { get; private set; }
        
        public async Task Load(Transform parent = null)
        {
            _handle = prefab.InstantiateAsync(parent);
            Component = (await _handle.Task).GetComponent<T>();
        }

        public void Unload()
        {
            if (!_handle.Equals(default))
            {
                Addressables.ReleaseInstance(_handle);
                _handle = default;
                Component = default;
            }
        }
    }
}