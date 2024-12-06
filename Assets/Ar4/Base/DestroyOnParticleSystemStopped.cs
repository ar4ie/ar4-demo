using UnityEngine;

namespace Ar4.Base
{
    public class DestroyOnParticleSystemStopped : MonoBehaviour
    {
        [SerializeField] GameObject target;
        
        void OnParticleSystemStopped()
        {
            Destroy(target);
        }
    }
}