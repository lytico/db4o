/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Tests.Common.Ext
{
	public class Db4oDatabaseTestCase : ITestCase
	{
		public virtual void TestGenerate()
		{
			Db4oDatabase db1 = Db4oDatabase.Generate();
			Db4oDatabase db2 = Db4oDatabase.Generate();
			Db4oDatabase db3 = Db4oDatabase.Generate();
			Assert.AreNotEqual(db1, db2);
			Assert.AreNotEqual(db1, db3);
			Assert.AreNotEqual(db2, db3);
		}
	}
}
