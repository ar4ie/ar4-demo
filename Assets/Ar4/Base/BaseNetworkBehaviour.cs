using Fusion;
using Zenject;

namespace Ar4.Base
{
    public class BaseNetworkBehaviour : NetworkBehaviour
    {
        public override void Spawned()
        {
            FindObjectOfType<SceneContext>().Container.Inject(this);
        }
    }
}