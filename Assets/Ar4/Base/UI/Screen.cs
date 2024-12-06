using UnityEngine;

namespace Ar4.Base.UI
{
    public abstract class Screen : MonoBehaviour {}

    public abstract class Screen<TModel> : Screen
    {
        protected TModel Model;

        public virtual void Init(TModel model)
        {
            Model = model;
        }
    }
}