using Ar4.Base;
using Ar4.Config;
using Ar4.Entities.Abilities;
using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Ar4.Entities.Players
{
    public class PlayerInputController : BaseNetworkBehaviour, IBeforeUpdate
    {
        [SerializeField] Player player;
        [Inject] GameConfig _gameConfig;
        [Inject] NetworkEvents _networkEvents;
        [Inject] InputActionAsset _inputActions;
        InputActionMap _inputActionMap;
        NetworkInputData _inputData;

        public override void Spawned()
        {
            base.Spawned();
            if (Object.HasInputAuthority)
            {
                _inputActionMap = _inputActions.FindActionMap("player");
                _inputActionMap.Enable();
                _networkEvents.OnInput.AddListener(OnInput);
            }
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            if (Object.HasInputAuthority)
            {
                _inputActionMap.Disable();
                _networkEvents.OnInput.RemoveListener(OnInput);
            }
            base.Despawned(runner, hasState);
        }

        public void BeforeUpdate()
        {
            if (!Object.HasInputAuthority)
                return;
            _inputData.ReadInput(_inputActionMap, _gameConfig);
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            input.Set(_inputData);
            _inputData = default;
        }

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();
            if (Object.IsProxy)
                return;
            if (GetInput(out NetworkInputData inputData))
            {
                if (!inputData.AbilityRequest.Equals(default(AbilityRequest)))
                    player.Entity.RequestAbility(inputData.AbilityRequest);
            }
        }
    }
}