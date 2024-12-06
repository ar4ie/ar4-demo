using Ar4.Config;
using Fusion;
using UnityEngine;

namespace Ar4.Entities.Abilities
{
    public struct AbilityRequest : INetworkStruct
    {
        public int AbilityId;
        public NetworkBehaviourId TargetEntityId;
        public Vector2 TargetPosition;

        public static AbilityRequest TickRequest(AbilityRequest newAbilityRequest, AbilityRequest abilityRequest, Entity source, GameConfig gameConfig)
        {
            var changed = newAbilityRequest.AbilityId != abilityRequest.AbilityId;
            if (changed)
            {
                var ability = gameConfig.GetAbility(abilityRequest.AbilityId);
                if (ability != null)
                    ability.TickStop(source);
            }
            abilityRequest = newAbilityRequest;
            if (changed)
            {
                var ability = gameConfig.GetAbility(abilityRequest.AbilityId);
                if (ability != null)
                    ability.TickStart(source);
            }
            return abilityRequest;
        }

        public static void RenderRequest(AbilityRequest newAbilityRequest, AbilityRequest abilityRequest, Entity source, GameConfig gameConfig)
        {
            var changed = newAbilityRequest.AbilityId != abilityRequest.AbilityId;
            if (changed)
            {
                var ability = gameConfig.GetAbility(abilityRequest.AbilityId);
                if (ability != null)
                    ability.RenderStop(source);
            }
            abilityRequest = newAbilityRequest;
            if (changed)
            {
                var ability = gameConfig.GetAbility(abilityRequest.AbilityId);
                if (ability != null)
                    ability.RenderStart(source);
            }
        }

        public static bool TickUpdate(AbilityRequest abilityRequest, Entity source, GameConfig gameConfig)
        {
            var ability = gameConfig.GetAbility(abilityRequest.AbilityId);
            if (ability == null)
                return false;
            Entity target;
            if (abilityRequest.TargetEntityId != NetworkBehaviourId.None)
            {
                if (!source.Runner.TryFindBehaviour<Entity>(abilityRequest.TargetEntityId, out target))
                    return true;
            }
            else
            {
                target = null;
            }
            return ability.TickUpdate(source, target, target != null ? target.Position : abilityRequest.TargetPosition);
        }
    }
}