/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Tests.Common.TA;

namespace Db4objects.Db4o.Tests.Common.TP
{
	public class Item : ActivatableImpl
	{
		public string name;

		public Item()
		{
		}

		public Item(string initialName)
		{
			name = initialName;
		}

		public virtual string GetName()
		{
			Activate(ActivationPurpose.Read);
			return name;
		}

		public virtual void SetName(string newName)
		{
			Activate(ActivationPurpose.Write);
			name = newName;
		}

		public override string ToString()
		{
			return "Item(" + GetName() + ")";
		}
	}
}
