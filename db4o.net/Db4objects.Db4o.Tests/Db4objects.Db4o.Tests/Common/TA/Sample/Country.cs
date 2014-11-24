/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Tests.Common.TA;
using Db4objects.Db4o.Tests.Common.TA.Sample;

namespace Db4objects.Db4o.Tests.Common.TA.Sample
{
	public class Country : ActivatableImpl
	{
		public State[] _states;

		public virtual State GetState(string zipCode)
		{
			Activate(ActivationPurpose.Read);
			return _states[0];
		}
	}
}
