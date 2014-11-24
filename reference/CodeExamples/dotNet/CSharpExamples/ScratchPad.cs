using System;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.TA;

namespace Db4oDoc
{
    /// <summary>
    /// <see cref="ActivationPurpose.Read"/>
    /// </summary>
    public class Item : IActivatable
    {
        private Item next;
        [NonSerialized] private IActivator activator;

        public void Bind(IActivator activator)
        {
            if (this.activator == activator)
            {
                return;
            }
            if (activator != null && null != this.activator)
            {
                throw new InvalidOperationException("Object can only be bound to one activator");
            }
            this.activator = activator;
        }

        public void Activate(ActivationPurpose activationPurpose)
        {
            if (null != activator)
            {
                activator.Activate(activationPurpose);
            }
        }

        public Item Next
        {
            get
            {
                Activate(ActivationPurpose.Read);
                return next;
            }
            set
            {
                Activate(ActivationPurpose.Write);
                next = value;
            }
        }

        // ...
    }
}