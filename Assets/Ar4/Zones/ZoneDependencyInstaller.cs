using Ar4.Config;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Ar4.Zones
{
    public class ZoneDependencyInstaller : MonoInstaller
    {
        [SerializeField] InputActionAsset inputActions;
        [SerializeField] GameConfig gameConfig;
        
        public override void InstallBindings()
        {
            Container.BindInstance(inputActions);
            Container.BindInstance(gameConfig);
        }
    }
}