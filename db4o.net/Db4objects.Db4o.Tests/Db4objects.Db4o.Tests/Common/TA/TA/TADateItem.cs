/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Tests.Common.TA;

namespace Db4objects.Db4o.Tests.Common.TA.TA
{
	public class TADateItem : ActivatableImpl
	{
		public const long Day = 1000 * 60 * 60 * 24;

		public DateTime _typed;

		public object _untyped;

		public virtual DateTime GetTyped()
		{
			Activate(ActivationPurpose.Read);
			return _typed;
		}

		public virtual object GetUntyped()
		{
			Activate(ActivationPurpose.Read);
			return _untyped;
		}

		public override string ToString()
		{
			Activate(ActivationPurpose.Read);
			return _typed.ToString();
		}
	}
}
