using System.Collections.Generic;
using Ar4.Entities;
using UnityEngine;

namespace Ar4.Base
{
    public static class PhysicsUtil
    {
        public static void Collide(Entity target, IEnumerable<Entity> sources, float collisionSpeed)
        {
            var shift = Vector2.zero;
            foreach (var source in sources)
            {
                if (source == target)
                    continue;
                var distance = target.Position - source.Position;
                var maxDistance = source.extend + target.extend;
                var speed = collisionSpeed * (1 - distance.SqrMagnitude() / (maxDistance * maxDistance));
                if (speed <= 0)
                    continue;
                var direction = distance.normalized;
                if (direction == Vector2.zero)
                    direction = Vector2.up;
                shift += direction * target.Runner.DeltaTime * speed;
            }
            target.Position += shift;
        }
    }
}