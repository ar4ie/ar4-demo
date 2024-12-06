using Ar4.Base;

namespace Ar4.Entities.Behaviours
{
    public abstract class BaseEntityBehaviour : BaseNetworkBehaviour
    {
        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();
            if (Object.IsProxy)
                return;
            TickUpdate();
        }

        protected abstract void TickUpdate();
    }
}