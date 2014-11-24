/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class IsStoredTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new IsStoredTestCase().RunConcurrency();
		}

		public string myString;

		public virtual void Conc(IExtObjectContainer oc)
		{
			IsStoredTestCase isStored = new IsStoredTestCase();
			isStored.myString = "isStored";
			oc.Store(isStored);
			Assert.IsTrue(oc.IsStored(isStored));
			oc.Commit();
			oc.Delete(isStored);
			Assert.IsFalse(oc.IsStored(isStored));
			oc.Rollback();
			Assert.IsTrue(oc.IsStored(isStored));
			oc.Delete(isStored);
			Assert.IsFalse(oc.IsStored(isStored));
			oc.Commit();
			Assert.IsFalse(oc.IsStored(isStored));
		}

		public virtual void Check(IExtObjectContainer oc)
		{
			AssertOccurrences(oc, typeof(IsStoredTestCase), 0);
		}
	}
}
#endif // !SILVERLIGHT
