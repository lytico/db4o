/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class PrefetchObjectCountZeroTestCase : AbstractDb4oTestCase, IOptOutAllButNetworkingCS
	{
		public class Item
		{
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ClientServer().PrefetchObjectCount(0);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new PrefetchObjectCountZeroTestCase.Item());
		}

		public virtual void TestZeroPrefetchObjectCount()
		{
			Assert.IsNotNull(((PrefetchObjectCountZeroTestCase.Item)RetrieveOnlyInstance(typeof(
				PrefetchObjectCountZeroTestCase.Item))));
		}
	}
}
#endif // !SILVERLIGHT
