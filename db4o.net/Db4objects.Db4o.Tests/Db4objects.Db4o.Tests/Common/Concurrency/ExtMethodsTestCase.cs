/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class ExtMethodsTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new ExtMethodsTestCase().RunConcurrency();
		}

		public virtual void Conc(IExtObjectContainer oc)
		{
			ExtMethodsTestCase em = new ExtMethodsTestCase();
			oc.Store(em);
			Assert.IsFalse(oc.IsClosed());
			Assert.IsTrue(oc.IsActive(em));
			Assert.IsTrue(oc.IsStored(em));
			oc.Deactivate(em, 1);
			Assert.IsTrue(!oc.IsActive(em));
			oc.Activate(em, 1);
			Assert.IsTrue(oc.IsActive(em));
			long id = oc.GetID(em);
			Assert.IsTrue(oc.IsCached(id));
			oc.Purge(em);
			Assert.IsFalse(oc.IsCached(id));
			Assert.IsFalse(oc.IsStored(em));
			Assert.IsFalse(oc.IsActive(em));
			oc.Bind(em, id);
			Assert.IsTrue(oc.IsCached(id));
			Assert.IsTrue(oc.IsStored(em));
			Assert.IsTrue(oc.IsActive(em));
			ExtMethodsTestCase em2 = (ExtMethodsTestCase)oc.GetByID(id);
			Assert.AreSame(em, em2);
			// Purge all and try again
			oc.Purge();
			Assert.IsTrue(oc.IsCached(id));
			Assert.IsTrue(oc.IsStored(em));
			Assert.IsTrue(oc.IsActive(em));
			em2 = (ExtMethodsTestCase)oc.GetByID(id);
			Assert.AreSame(em, em2);
			oc.Delete(em2);
			oc.Commit();
			Assert.IsFalse(oc.IsCached(id));
			Assert.IsFalse(oc.IsStored(em2));
			Assert.IsFalse(oc.IsActive(em2));
			// Null checks
			Assert.IsFalse(oc.IsStored(null));
			Assert.IsFalse(oc.IsActive(null));
			Assert.IsFalse(oc.IsCached(0));
		}
	}
}
#endif // !SILVERLIGHT
