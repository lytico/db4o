/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
using System;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.TA;

namespace Db4objects.Db4o.Tests.CLI2.Collections.Transparent
{
	[Serializable]
	public class ActivatableElement : AbstractCollectionElement, IActivatable
	{
		public ActivatableElement(string name) : base(name)
		{
		}

		public void Bind(IActivator activator)
		{
			if (activator == _activator)
			{
				return;
			}

			if (activator != null && _activator != null)
			{
				throw new InvalidOperationException();
			}

			_activator = activator;
		}

		public void Activate(ActivationPurpose purpose)
		{
			if (_activator == null) return;
			_activator.Activate(purpose);
		}

		public override bool Equals(object obj)
		{
			ActivatableElement other = obj as ActivatableElement;
			if (other == null || other.GetType() != typeof(ActivatableElement))
			{
				return false;
			}
			
			Activate(ActivationPurpose.Read);

			return Name.Equals(other.Name);
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}
	
		public override string ToString()
		{
			Activate(ActivationPurpose.Read);
			return "ActivatableElement " + Name;
		}

		protected override void ReadFieldAccess()
		{
			Activate(ActivationPurpose.Read);
		}

		[NonSerialized]
		private IActivator _activator;
	}
}
