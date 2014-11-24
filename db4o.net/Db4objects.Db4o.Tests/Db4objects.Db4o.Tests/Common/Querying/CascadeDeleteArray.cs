/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class CascadeDeleteArray : AbstractDb4oTestCase
	{
		public class ArrayElem
		{
			public string name;

			public ArrayElem(string name)
			{
				this.name = name;
			}
		}

		public CascadeDeleteArray.ArrayElem[] array;

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(this).CascadeOnDelete(true);
		}

		protected override void Store()
		{
			CascadeDeleteArray cda = new CascadeDeleteArray();
			cda.array = new CascadeDeleteArray.ArrayElem[] { new CascadeDeleteArray.ArrayElem
				("one"), new CascadeDeleteArray.ArrayElem("two"), new CascadeDeleteArray.ArrayElem
				("three") };
			Db().Store(cda);
		}

		public virtual void Test()
		{
			CascadeDeleteArray cda = (CascadeDeleteArray)((CascadeDeleteArray)RetrieveOnlyInstance
				(GetType()));
			Assert.AreEqual(3, CountOccurences(typeof(CascadeDeleteArray.ArrayElem)));
			Db().Delete(cda);
			Assert.AreEqual(0, CountOccurences(typeof(CascadeDeleteArray.ArrayElem)));
			Db().Rollback();
			Assert.AreEqual(3, CountOccurences(typeof(CascadeDeleteArray.ArrayElem)));
			Db().Delete(cda);
			Assert.AreEqual(0, CountOccurences(typeof(CascadeDeleteArray.ArrayElem)));
			Db().Commit();
			Assert.AreEqual(0, CountOccurences(typeof(CascadeDeleteArray.ArrayElem)));
		}
	}
}
