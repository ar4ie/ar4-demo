using UnityEngine;

namespace Ar4.Entities.Models
{
    public class EntityModel : MonoBehaviour
    {
        [SerializeField] Animator animator;
        static readonly int ParamAction = Animator.StringToHash("Action");
        static readonly int ParamActionTrigger = Animator.StringToHash("ActionTrigger");
        [SerializeField] GameObject despawnEffectPrefab;

        public void PlayAction(EAction action)
        {
            animator.SetInteger(ParamAction, (int) action);
            animator.SetBool(ParamActionTrigger, true);
        }

        public enum EAction
        {
            AttackMelee = 1,
        }

        public void OnDespawn()
        {
            Instantiate(despawnEffectPrefab, transform.position, transform.rotation);
        }
    }
}