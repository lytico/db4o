/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Assorted;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class DualDeleteTestCase : Db4oClientServerTestCase
	{
		public class Item
		{
			public Atom atom;
		}

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(DualDeleteTestCase.Item)).CascadeOnDelete(true);
			config.ObjectClass(typeof(DualDeleteTestCase.Item)).CascadeOnUpdate(true);
		}

		protected override void Store()
		{
			DualDeleteTestCase.Item dd1 = new DualDeleteTestCase.Item();
			dd1.atom = new Atom("justone");
			Store(dd1);
			DualDeleteTestCase.Item dd2 = new DualDeleteTestCase.Item();
			dd2.atom = dd1.atom;
			Store(dd2);
		}

		public virtual void TestSingleSession()
		{
			DeleteAll(typeof(DualDeleteTestCase.Item));
			AssertOccurrences(typeof(Atom), 0);
			Db().Rollback();
			AssertOccurrences(typeof(Atom), 1);
			DeleteAll(typeof(DualDeleteTestCase.Item));
			AssertOccurrences(typeof(Atom), 0);
			Db().Commit();
			AssertOccurrences(typeof(Atom), 0);
			Db().Rollback();
			AssertOccurrences(typeof(Atom), 0);
		}

		public virtual void TestSeparateSessions()
		{
			IExtObjectContainer oc1 = OpenNewSession();
			IExtObjectContainer oc2 = OpenNewSession();
			try
			{
				IObjectSet os1 = oc1.Query(typeof(DualDeleteTestCase.Item));
				IObjectSet os2 = oc2.Query(typeof(DualDeleteTestCase.Item));
				DeleteObjectSet(oc1, os1);
				AssertOccurrences(oc1, typeof(Atom), 0);
				AssertOccurrences(oc2, typeof(Atom), 1);
				DeleteObjectSet(oc2, os2);
				AssertOccurrences(oc1, typeof(Atom), 0);
				AssertOccurrences(oc2, typeof(Atom), 0);
				oc1.Rollback();
				AssertOccurrences(oc1, typeof(Atom), 1);
				AssertOccurrences(oc2, typeof(Atom), 0);
				oc1.Commit();
				AssertOccurrences(oc1, typeof(Atom), 1);
				AssertOccurrences(oc2, typeof(Atom), 0);
				DeleteAll(oc2, typeof(DualDeleteTestCase.Item));
				oc2.Commit();
				AssertOccurrences(oc1, typeof(Atom), 0);
				AssertOccurrences(oc2, typeof(Atom), 0);
			}
			finally
			{
				oc1.Close();
				oc2.Close();
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Conc1(IExtObjectContainer oc)
		{
			IObjectSet os = oc.Query(typeof(DualDeleteTestCase.Item));
			Thread.Sleep(500);
			DeleteObjectSet(oc, os);
			oc.Rollback();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Check1(IExtObjectContainer oc)
		{
			AssertOccurrences(oc, typeof(Atom), 1);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Conc2(IExtObjectContainer oc)
		{
			IObjectSet os = oc.Query(typeof(DualDeleteTestCase.Item));
			Thread.Sleep(500);
			DeleteObjectSet(oc, os);
			AssertOccurrences(oc, typeof(Atom), 0);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Check2(IExtObjectContainer oc)
		{
			AssertOccurrences(oc, typeof(Atom), 0);
		}
	}
}
