/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Tests.Common.TA;

namespace Db4objects.Db4o.Tests.Jre5.Collections
{
	public class Product : ActivatableImpl
	{
		private string _code;

		private string _description;

		public Product(string code, string description)
		{
			_code = code;
			_description = description;
		}

		public virtual string Code()
		{
			Activate(ActivationPurpose.Read);
			return _code;
		}

		public virtual string Description()
		{
			Activate(ActivationPurpose.Read);
			return _description;
		}

		public override bool Equals(object p)
		{
			Activate(ActivationPurpose.Read);
			if (p == null)
			{
				return false;
			}
			if (p.GetType() != this.GetType())
			{
				return false;
			}
			Db4objects.Db4o.Tests.Jre5.Collections.Product rhs = (Db4objects.Db4o.Tests.Jre5.Collections.Product
				)p;
			return rhs._code == _code;
		}
	}
}
