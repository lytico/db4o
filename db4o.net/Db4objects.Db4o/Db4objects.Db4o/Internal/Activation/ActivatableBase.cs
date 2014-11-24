/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.TA;

namespace Db4objects.Db4o.Internal.Activation
{
	public abstract class ActivatableBase : IActivatable
	{
		[System.NonSerialized]
		private IActivator _activator;

		public virtual void Bind(IActivator activator)
		{
			if (_activator == activator)
			{
				return;
			}
			if (activator != null && _activator != null)
			{
				throw new InvalidOperationException();
			}
			_activator = activator;
		}

		public virtual void Activate(ActivationPurpose purpose)
		{
			if (_activator == null)
			{
				return;
			}
			_activator.Activate(purpose);
		}

		protected virtual void ActivateForRead()
		{
			Activate(ActivationPurpose.Read);
		}

		protected virtual void ActivateForWrite()
		{
			Activate(ActivationPurpose.Write);
		}
	}
}
