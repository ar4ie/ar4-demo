using UnityEngine;

namespace Ar4.Entities.Abilities
{
    public abstract class Ability : ScriptableObject
    {
        public int id;

        public virtual void TickStart(Entity sourceEntity) {}

        public virtual void RenderStart(Entity sourceEntity) {}

        public virtual void TickStop(Entity sourceEntity) {}

        public virtual void RenderStop(Entity sourceEntity) {}

        /**
         * <param name="sourceEntity">action source entity</param>
         * <param name="targetEntity">action target entity, if present</param>
         * <param name="targetPosition">action fallback target position</param>
         * <returns>true if action finished</returns>
         */
        public abstract bool TickUpdate(Entity sourceEntity, Entity targetEntity, Vector2 targetPosition);
    }
}