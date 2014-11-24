/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class TransientCloneTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public IList list;

			public Hashtable ht;

			public string str;

			public int myInt;

			public Molecule[] molecules;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			TransientCloneTestCase.Item item = new TransientCloneTestCase.Item();
			item.list = new ArrayList();
			item.list.Add(new Atom("listAtom"));
			item.list.Add(item);
			item.ht = new Hashtable();
			item.ht["htc"] = new Molecule("htAtom");
			item.ht["recurse"] = item;
			item.str = "str";
			item.myInt = 100;
			item.molecules = new Molecule[3];
			for (int i = 0; i < item.molecules.Length; i++)
			{
				item.molecules[i] = new Molecule("arr" + i);
				item.molecules[i].child = new Atom("arr" + i);
				item.molecules[i].child.child = new Atom("arrc" + i);
			}
			Store(item);
		}

		public virtual void Test()
		{
			TransientCloneTestCase.Item item = ((TransientCloneTestCase.Item)RetrieveOnlyInstance
				(typeof(TransientCloneTestCase.Item)));
			Db().Activate(item, int.MaxValue);
			TransientCloneTestCase.Item originalValues = PeekPersisted(false);
			Cmp(item, originalValues);
			Db().Deactivate(item, int.MaxValue);
			TransientCloneTestCase.Item modified = PeekPersisted(false);
			Cmp(originalValues, modified);
			Db().Activate(item, int.MaxValue);
			modified.str = "changed";
			modified.molecules[0].name = "changed";
			item.str = "changed";
			item.molecules[0].name = "changed";
			Db().Store(item.molecules[0]);
			Db().Store(item);
			TransientCloneTestCase.Item tc = PeekPersisted(true);
			Cmp(originalValues, tc);
			tc = PeekPersisted(false);
			Cmp(modified, tc);
			Db().Commit();
			tc = PeekPersisted(true);
			Cmp(modified, tc);
		}

		private void Cmp(TransientCloneTestCase.Item to, TransientCloneTestCase.Item tc)
		{
			Assert.IsTrue(tc != to);
			Assert.IsTrue(tc.list != to);
			Assert.IsTrue(tc.list.Count == to.list.Count);
			IEnumerator i = tc.list.GetEnumerator();
			Atom tca = ((Atom)Next(i));
			IEnumerator j = to.list.GetEnumerator();
			Atom tct = ((Atom)Next(j));
			Assert.IsTrue(tca != tct);
			Assert.IsTrue(tca.name.Equals(tct.name));
			Assert.AreSame(Next(i), tc);
			Assert.AreSame(Next(j), to);
			Assert.IsTrue(tc.ht != to.ht);
			Molecule tcm = (Molecule)tc.ht["htc"];
			Molecule tom = (Molecule)to.ht["htc"];
			Assert.IsTrue(tcm != tom);
			Assert.IsTrue(tcm.name.Equals(tom.name));
			Assert.AreSame(tc.ht["recurse"], tc);
			Assert.AreSame(to.ht["recurse"], to);
			Assert.AreEqual(to.str, tc.str);
			Assert.IsTrue(tc.str.Equals(to.str));
			Assert.IsTrue(tc.myInt == to.myInt);
			Assert.IsTrue(tc.molecules.Length == to.molecules.Length);
			Assert.IsTrue(tc.molecules.Length == to.molecules.Length);
			tcm = tc.molecules[0];
			tom = to.molecules[0];
			Assert.IsTrue(tcm != tom);
			Assert.IsTrue(tcm.name.Equals(tom.name));
			Assert.IsTrue(tcm.child != tom.child);
			Assert.IsTrue(tcm.child.name.Equals(tom.child.name));
		}

		private object Next(IEnumerator i)
		{
			Assert.IsTrue(i.MoveNext());
			return (object)i.Current;
		}

		private TransientCloneTestCase.Item PeekPersisted(bool committed)
		{
			IExtObjectContainer oc = Db();
			return ((TransientCloneTestCase.Item)oc.PeekPersisted(((TransientCloneTestCase.Item
				)RetrieveOnlyInstance(typeof(TransientCloneTestCase.Item))), int.MaxValue, committed
				));
		}
	}
}
