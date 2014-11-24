/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Types;

namespace Db4objects.Db4o.Tests.Common.Types
{
	public class StoreTopLevelPrimitiveTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new StoreTopLevelPrimitiveTestCase().RunAll();
		}

		public virtual void Test()
		{
			bool exceptionHappened = false;
			try
			{
				Store(42);
			}
			catch (ObjectNotStorableException onsex)
			{
				exceptionHappened = true;
				StringAssert.Contains("Value types", onsex.Message);
			}
			Assert.IsTrue(exceptionHappened);
		}
	}
}
