/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Experiments
{
	public class STNullOnPathTestCase : SodaBaseTestCase
	{
		public bool @bool;

		public STNullOnPathTestCase()
		{
		}

		public STNullOnPathTestCase(bool @bool)
		{
			this.@bool = @bool;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Experiments.STNullOnPathTestCase
				(false) };
		}

		public virtual void Test()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Experiments.STNullOnPathTestCase
				());
			q.Descend("bool").Constrain(null);
			Expect(q, new int[] {  });
		}
	}
}
