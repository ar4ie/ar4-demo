using Ar4.Config;
using Ar4.Entities.Abilities;
using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ar4.Entities.Players
{
    public struct NetworkInputData : INetworkInput
    {
        public AbilityRequest AbilityRequest;

        public void ReadInput(InputActionMap inputActionMap, GameConfig gameConfig)
        {
            foreach (var inputAbilityConfig in gameConfig.playerInputAbility)
            {
                if (inputActionMap.FindAction(inputAbilityConfig.inputAction).IsPressed())
                {
                    var inputPosition = inputActionMap.FindAction("look").ReadValue<Vector2>();
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(inputPosition), out var hit))
                    {
                        var target = hit.collider.transform.GetComponentInParent<Entity>();
                        AbilityRequest = new AbilityRequest()
                        {
                            AbilityId = inputAbilityConfig.ability.id,
                            TargetEntityId = target ? target.Id : NetworkBehaviourId.None,
                            TargetPosition = new Vector2(hit.point.x, hit.point.z)
                        };
                    }
                }
            }
        }
    }
}