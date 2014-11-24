/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Concurrency;
using Db4objects.Db4o.Tests.Common.Persistent;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class CascadeToVectorTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new CascadeToVectorTestCase().RunConcurrency();
		}

		public ArrayList vec;

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(this).CascadeOnUpdate(true);
			config.ObjectClass(this).CascadeOnDelete(true);
			config.ObjectClass(typeof(Atom)).CascadeOnDelete(false);
		}

		protected override void Store()
		{
			CascadeToVectorTestCase ctv = new CascadeToVectorTestCase();
			ctv.vec = new ArrayList();
			ctv.vec.Add(new Atom("stored1"));
			ctv.vec.Add(new Atom(new Atom("storedChild1"), "stored2"));
			Store(ctv);
		}

		public virtual void Conc(IExtObjectContainer oc)
		{
			CascadeToVectorTestCase ctv = (CascadeToVectorTestCase)((CascadeToVectorTestCase)
				RetrieveOnlyInstance(oc, typeof(CascadeToVectorTestCase)));
			IEnumerator i = ctv.vec.GetEnumerator();
			while (i.MoveNext())
			{
				Atom atom = (Atom)i.Current;
				atom.name = "updated";
				if (atom.child != null)
				{
					// This one should NOT cascade
					atom.child.name = "updated";
				}
			}
			oc.Store(ctv);
		}

		public virtual void Check(IExtObjectContainer oc)
		{
			CascadeToVectorTestCase ctv = (CascadeToVectorTestCase)((CascadeToVectorTestCase)
				RetrieveOnlyInstance(oc, typeof(CascadeToVectorTestCase)));
			IEnumerator i = ctv.vec.GetEnumerator();
			while (i.MoveNext())
			{
				Atom atom = (Atom)i.Current;
				Assert.AreEqual("updated", atom.name);
				if (atom.child != null)
				{
					Assert.AreEqual("storedChild1", atom.child.name);
				}
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void ConcDelete(IExtObjectContainer oc, int seq)
		{
			IObjectSet os = oc.Query(typeof(CascadeToVectorTestCase));
			if (os.Count == 0)
			{
				// already deleted
				return;
			}
			Assert.AreEqual(1, os.Count);
			CascadeToVectorTestCase ctv = (CascadeToVectorTestCase)os.Next();
			// wait for other threads
			Thread.Sleep(500);
			oc.Delete(ctv);
		}

		public virtual void CheckDelete(IExtObjectContainer oc)
		{
			// Cascade-On-Delete Test: We only want one atom to remain.
			AssertOccurrences(oc, typeof(Atom), 1);
		}
	}
}
#endif // !SILVERLIGHT
