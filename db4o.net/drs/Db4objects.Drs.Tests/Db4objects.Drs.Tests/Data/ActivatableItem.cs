/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Activation;
using Db4objects.Db4o.TA;

namespace Db4objects.Drs.Tests.Data
{
	/// <exclude></exclude>
	public class ActivatableItem : IActivatable
	{
		private string name;

		[System.NonSerialized]
		private IActivator _activator;

		public ActivatableItem()
		{
		}

		public ActivatableItem(string name)
		{
			this.name = name;
		}

		public virtual void Activate(ActivationPurpose purpose)
		{
			if (_activator != null)
			{
				_activator.Activate(purpose);
			}
		}

		public virtual void Bind(IActivator activator)
		{
			_activator = activator;
		}

		public virtual object Name()
		{
			Activate(ActivationPurpose.Read);
			return name;
		}

		public virtual string GetName()
		{
			Activate(ActivationPurpose.Read);
			return name;
		}

		public virtual void SetName(string name)
		{
			Activate(ActivationPurpose.Write);
			this.name = name;
		}
	}
}
