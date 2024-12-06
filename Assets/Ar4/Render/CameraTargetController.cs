using UnityEngine;

namespace Ar4.Render
{
    public class CameraTargetController : MonoBehaviour
    {
        public GameObject target;
        public Vector3 distance;

        public void Update()
        {
            transform.position = target.transform.position + distance;
        }
    }
}