using System;
using System.Collections.Generic;
using Ar4.Entities.Models;
using Fusion;
using UnityEngine;

namespace Ar4.Entities.Abilities
{
    [CreateAssetMenu(fileName = "AttackMeleeAbility", menuName = "Ar4/Entities/Actions/AttackMeleeAbility")]
    public class AttackMeleeAbility : Ability
    {
        [SerializeField] float duration;
        [SerializeField] float hitTime;
        [SerializeField] float range;
        [SerializeField] float radius;
        [SerializeField] EntityModel.EAction modelAction;
        static List<Entity> _entitiesTemp = new ();

        public override void TickStart(Entity sourceEntity)
        {
            sourceEntity.ActionTimer = TickTimer.CreateFromSeconds(sourceEntity.Runner, duration);
        }

        public override void RenderStart(Entity sourceEntity)
        {
            sourceEntity.Model.PlayAction(modelAction);
        }

        public override bool TickUpdate(Entity sourceEntity, Entity targetEntity, Vector2 targetPosition)
        {
            var distance = targetPosition - sourceEntity.Position;
            sourceEntity.Rotation = Mathf.Atan2(distance.x, distance.y);
            if (sourceEntity.ActionTimer.TargetTick == TickTimer.CreateFromSeconds(sourceEntity.Runner, duration - hitTime).TargetTick)
                Hit(sourceEntity, sourceEntity.Position + distance.normalized * range);
            return sourceEntity.ActionTimer.Expired(sourceEntity.Runner);
        }

        void Hit(Entity sourceEntity, Vector2 targetPosition)
        {
            foreach (var target in sourceEntity.Zone.Entities.Values)
            {
                if (target == sourceEntity)
                    continue;
                var hitDistance = target.Position - targetPosition;
                var maxHitDistance = radius + target.extend;
                if (hitDistance.SqrMagnitude() > maxHitDistance * maxHitDistance)
                    continue;
                _entitiesTemp.Add(target);
            }
            foreach (var target in _entitiesTemp)
                target.Hit(sourceEntity);
            _entitiesTemp.Clear();
        }
    }
}