using System;
using UnityEngine;

namespace Ar4.Entities.Abilities
{
    [CreateAssetMenu(fileName = "MoveAbility", menuName = "Ar4/Entities/Actions/MoveAbility")]
    public class MoveAbility : Ability
    {
        [SerializeField] float stopDistance;
        
        public override bool TickUpdate(Entity sourceEntity, Entity targetEntity, Vector2 targetPosition)
        {
            var distance = targetPosition - sourceEntity.Position;
            var requiredDistance = sourceEntity.extend + (targetEntity != null ? targetEntity.extend : 0) + stopDistance;
            var distanceLeft = distance.magnitude - requiredDistance;
            if (distanceLeft <= 0)
                return true;
            sourceEntity.Position += distance.normalized * Math.Min(sourceEntity.Runner.DeltaTime * sourceEntity.moveSpeed, distanceLeft);
            sourceEntity.Rotation = Mathf.Atan2(distance.x, distance.y);
            return false;
        }
    }
}