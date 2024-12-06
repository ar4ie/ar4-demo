using UnityEngine;

namespace Ar4.Render
{
    public class RenderController : MonoBehaviour
    {
        [SerializeField] new Camera camera;
        [SerializeField] Vector3 cameraTargetDistance;
        CameraTargetController _cameraTargetController;

        void Awake() {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 60;
        }

        void OnDestroy()
        {
            DestroyCameraController();
        }

        public void SetCameraTarget(GameObject target)
        {
            DestroyCameraController();
            CreateCameraController(target);
        }

        void CreateCameraController(GameObject target)
        {
            if (target != null)
            {
                _cameraTargetController = camera.gameObject.AddComponent<CameraTargetController>();
                _cameraTargetController.target = target;
                _cameraTargetController.distance = cameraTargetDistance;
            }
        }

        void DestroyCameraController()
        {
            if (_cameraTargetController != null)
            {
                Destroy(_cameraTargetController);
                _cameraTargetController = null;
            }
        }
    }
}