using Ar4.Entities.Abilities;
using Ar4.Zones;
using UnityEngine;
using Zenject;

namespace Ar4.Entities.Behaviours
{
    public class FollowEntityBehaviour : BaseEntityBehaviour
    {
        [Inject] Zone _zone;
        [SerializeField] Entity entity;
        [SerializeField] Ability moveAbility;

        protected override void TickUpdate()
        {
            Entity target = null;
            var distanceSqr = 20f * 20f;
            foreach (var t in _zone.Entities.Values)
            {
                if (!entity.IsEnemy(t))
                    continue;
                var dSqr = (t.Position - entity.Position).SqrMagnitude();
                if (dSqr < distanceSqr)
                {
                    target = t;
                    distanceSqr = dSqr;
                }
            }
            if (target != null)
                entity.RequestAbility(new AbilityRequest()
                {
                    AbilityId = moveAbility.id,
                    TargetEntityId = target.Id
                });
            else
                entity.RequestAbility(default);
        }
    }
}