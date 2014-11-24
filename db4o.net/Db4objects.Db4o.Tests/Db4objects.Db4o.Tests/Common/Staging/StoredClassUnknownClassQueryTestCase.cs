/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Staging;

namespace Db4objects.Db4o.Tests.Common.Staging
{
	public class StoredClassUnknownClassQueryTestCase : AbstractDb4oTestCase
	{
		public class UnknownClass
		{
			public int _id;
		}

		public virtual void Test()
		{
			int numStoredClasses = Db().StoredClasses().Length;
			Assert.IsNull(Db().StoredClass(typeof(StoredClassUnknownClassQueryTestCase.UnknownClass
				)));
			Assert.AreEqual(0, Db().Query(typeof(StoredClassUnknownClassQueryTestCase.UnknownClass
				)).Count);
			Assert.IsNull(Db().StoredClass(typeof(StoredClassUnknownClassQueryTestCase.UnknownClass
				)));
			Assert.AreEqual(numStoredClasses, Db().StoredClasses().Length);
		}
	}
}
