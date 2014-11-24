/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using System.Collections.Generic;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Collections;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Jre12.Collections;
using Db4objects.Db4o.Typehandlers;
using Sharpen.Util;

namespace Db4objects.Db4o.Tests.Jre12.Collections
{
	public class BigSetTestCase : AbstractDb4oTestCase, IOptOutMultiSession
	{
		public static void Main(string[] args)
		{
			new BigSetTestCase().RunSolo("testBigSetAfterCommit");
		}

		private static readonly BigSetTestCase.Item ItemOne = new BigSetTestCase.Item("one"
			);

		private static readonly BigSetTestCase.Item[] items = new BigSetTestCase.Item[] { 
			new BigSetTestCase.Item("one"), new BigSetTestCase.Item("two"), new BigSetTestCase.Item
			("three") };

		public class Holder<E>
		{
			public Db4objects.Db4o.Collections.ISet<E> _set;
		}

		public class Item
		{
			public string _name;

			public Item(string name)
			{
				_name = name;
			}

			public override bool Equals(object obj)
			{
				if (!(obj is BigSetTestCase.Item))
				{
					return false;
				}
				BigSetTestCase.Item other = (BigSetTestCase.Item)obj;
				if (_name == null)
				{
					return other._name == null;
				}
				return _name.Equals(other._name);
			}

			public override string ToString()
			{
				return "Item(" + _name + ")";
			}
		}

		public virtual void TestRefreshBigSet()
		{
			BigSetTestCase.Holder<BigSetTestCase.Item> holder = NewHolderWithBigSet(new BigSetTestCase.Item
				("1"), new BigSetTestCase.Item("2"));
			StoreAndCommit(holder);
			Db().Refresh(holder, int.MaxValue);
			Assert.AreEqual(2, holder._set.Count);
		}

		public virtual void TestAddAfterCommit()
		{
			RunTestAfterCommit(new _IProcedure4_70());
		}

		private sealed class _IProcedure4_70 : IProcedure4
		{
			public _IProcedure4_70()
			{
			}

			public void Apply(object set)
			{
				((Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item>)set).Add(new BigSetTestCase.Item
					("3"));
			}
		}

		private void RunTestAfterCommit(IProcedure4 setOperations)
		{
			BigSetTestCase.Holder<BigSetTestCase.Item> holder = NewHolderWithBigSet(new BigSetTestCase.Item
				("1"), new BigSetTestCase.Item("2"));
			StoreAndCommit(holder);
			Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item> set = holder._set;
			Assert.AreEqual(2, set.Count);
			setOperations.Apply(set);
			PurgeAll(holder, holder._set);
			BigSetTestCase.Holder<BigSetTestCase.Item> resurrected = (BigSetTestCase.Holder<BigSetTestCase.Item
				>)RetrieveOnlyInstance(holder.GetType());
			IteratorAssert.SameContent(set.GetEnumerator(), resurrected._set.GetEnumerator());
		}

		public virtual void TestClearAfterCommit()
		{
			RunTestAfterCommit(new _IProcedure4_92());
		}

		private sealed class _IProcedure4_92 : IProcedure4
		{
			public _IProcedure4_92()
			{
			}

			public void Apply(object set)
			{
				((Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item>)set).Clear();
			}
		}

		public virtual void TestRemoveAfterCommit()
		{
			RunTestAfterCommit(new _IProcedure4_100(this));
		}

		private sealed class _IProcedure4_100 : IProcedure4
		{
			public _IProcedure4_100(BigSetTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Apply(object set)
			{
				((Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item>)set).Remove(this._enclosing
					.QueryItem("1"));
			}

			private readonly BigSetTestCase _enclosing;
		}

		protected virtual BigSetTestCase.Item QueryItem(string name)
		{
			IQuery query = NewQuery(typeof(BigSetTestCase.Item));
			query.Descend("_name").Constrain(name);
			return (BigSetTestCase.Item)query.Execute()[0];
		}

		private void StoreAndCommit(BigSetTestCase.Holder<BigSetTestCase.Item> holder)
		{
			Store(holder);
			Db().Commit();
		}

		public virtual void TestPurgeBeforeCommit()
		{
			BigSetTestCase.Holder<BigSetTestCase.Item> holder = NewHolderWithBigSet(new BigSetTestCase.Item
				("foo"));
			Store(holder);
			PurgeAll(holder, holder._set);
			holder = (BigSetTestCase.Holder<BigSetTestCase.Item>)RetrieveOnlyInstance(holder.
				GetType());
			Assert.AreEqual(1, holder._set.Count);
		}

		private BigSetTestCase.Holder<BigSetTestCase.Item> NewHolderWithBigSet(params BigSetTestCase.Item
			[] item)
		{
			BigSetTestCase.Holder<BigSetTestCase.Item> holder = new BigSetTestCase.Holder<BigSetTestCase.Item
				>();
			holder._set = NewBigSet(item);
			return holder;
		}

		private void PurgeAll(params object[] objects)
		{
			foreach (object @object in objects)
			{
				Db().Purge(@object);
			}
		}

		public virtual void TestTypeHandlerInstalled()
		{
			ITypeHandler4 typeHandler = Container().Handlers.ConfiguredTypeHandler(Reflector(
				).ForClass(NewBigSet().GetType()));
			Assert.IsInstanceOf(typeof(BigSetTypeHandler), typeHandler);
		}

		public virtual void TestEmptySet()
		{
			Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item> set = NewBigSet();
			Assert.AreEqual(0, set.Count);
		}

		public virtual void TestSize()
		{
			Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item> set = NewBigSet();
			set.Add(ItemOne);
			Assert.AreEqual(1, set.Count);
			set.Remove(ItemOne);
			Assert.AreEqual(0, set.Count);
			BigSetTestCase.Item itemTwo = new BigSetTestCase.Item("two");
			set.Add(itemTwo);
			set.Add(new BigSetTestCase.Item("three"));
			Assert.AreEqual(2, set.Count);
			set.Remove(itemTwo);
			Assert.AreEqual(1, set.Count);
		}

		public virtual void TestContains()
		{
			Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item> set = NewBigSet();
			set.Add(ItemOne);
			Assert.IsTrue(set.Contains(ItemOne));
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestPersistence()
		{
			BigSetTestCase.Holder<BigSetTestCase.Item> holder = new BigSetTestCase.Holder<BigSetTestCase.Item
				>();
			holder._set = NewBigSet();
			Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item> set = holder._set;
			set.Add(ItemOne);
			Store(holder);
			Reopen();
			holder = (BigSetTestCase.Holder<BigSetTestCase.Item>)RetrieveOnlyInstance(holder.
				GetType());
			set = holder._set;
			AssertSinglePersistentItem(set);
		}

		private void AssertSinglePersistentItem(Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item
			> set)
		{
			BigSetTestCase.Item expectedItem = (BigSetTestCase.Item)RetrieveOnlyInstance(typeof(
				BigSetTestCase.Item));
			Assert.IsNotNull(set);
			Assert.AreEqual(1, set.Count);
			IEnumerator setIterator = set.GetEnumerator();
			Assert.IsNotNull(setIterator);
			Assert.IsTrue(setIterator.MoveNext());
			BigSetTestCase.Item actualItem = (BigSetTestCase.Item)setIterator.Current;
			Assert.AreSame(expectedItem, actualItem);
		}

		public virtual void TestAddAllContainsAll()
		{
			Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item> set = NewBigSet();
			IList<BigSetTestCase.Item> collection = ItemList();
			Assert.IsTrue(Sharpen.Collections.AddAll(set, collection));
			Assert.IsTrue(set.ContainsAll(collection));
			Assert.IsFalse(Sharpen.Collections.AddAll(set, collection));
			Assert.AreEqual(collection.Count, set.Count);
		}

		public virtual void TestRemove()
		{
			Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item> set = NewBigSet();
			IList<BigSetTestCase.Item> collection = ItemList();
			Sharpen.Collections.AddAll(set, collection);
			BigSetTestCase.Item first = collection[0];
			set.Remove(first);
			Assert.IsTrue(collection.Remove(first));
			Assert.IsFalse(collection.Remove(first));
			Assert.IsTrue(set.ContainsAll(collection));
			Assert.IsFalse(set.Contains(first));
		}

		public virtual void TestRemoveAll()
		{
			Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item> set = NewBigSet();
			IList<BigSetTestCase.Item> collection = ItemList();
			Sharpen.Collections.AddAll(set, collection);
			Assert.IsTrue(set.RemoveAll(collection));
			Assert.AreEqual(0, set.Count);
			Assert.IsFalse(set.RemoveAll(collection));
		}

		public virtual void TestIsEmpty()
		{
			Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item> set = NewBigSet();
			Assert.IsTrue(set.IsEmpty);
			set.Add(ItemOne);
			Assert.IsFalse(set.IsEmpty);
			set.Remove(ItemOne);
			Assert.IsTrue(set.IsEmpty);
		}

		public virtual void TestIterator()
		{
			Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item> set = NewBigSet();
			ICollection<BigSetTestCase.Item> collection = ItemList();
			Sharpen.Collections.AddAll(set, collection);
			IEnumerator i = set.GetEnumerator();
			Assert.IsNotNull(i);
			IteratorAssert.SameContent(collection.GetEnumerator(), i);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestDelete()
		{
			Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item> set = NewBigSet();
			set.Add(ItemOne);
			Db().Store(set);
			Db().Commit();
			BTree bTree = BTree(set);
			BTreeAssert.AssertAllSlotsFreed(FileTransaction(), bTree, new _ICodeBlock_259(this
				, set));
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_265(set));
		}

		private sealed class _ICodeBlock_259 : ICodeBlock
		{
			public _ICodeBlock_259(BigSetTestCase _enclosing, Db4objects.Db4o.Collections.ISet
				<BigSetTestCase.Item> set)
			{
				this._enclosing = _enclosing;
				this.set = set;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.Db().Delete(set);
				this._enclosing.Db().Commit();
			}

			private readonly BigSetTestCase _enclosing;

			private readonly Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item> set;
		}

		private sealed class _ICodeBlock_265 : ICodeBlock
		{
			public _ICodeBlock_265(Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item> set)
			{
				this.set = set;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				set.Add(BigSetTestCase.ItemOne);
			}

			private readonly Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item> set;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestDefragment()
		{
			Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item> set = NewBigSet();
			set.Add(ItemOne);
			Db().Store(set);
			Db().Commit();
			Defragment();
			set = (Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item>)RetrieveOnlyInstance
				(set.GetType());
			AssertSinglePersistentItem(set);
		}

		public virtual void TestClear()
		{
			Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item> set = NewBigSet();
			set.Add(ItemOne);
			set.Clear();
			Assert.AreEqual(0, set.Count);
		}

		private IList<BigSetTestCase.Item> ItemList()
		{
			IList<BigSetTestCase.Item> c = new List<BigSetTestCase.Item>();
			for (int i = 0; i < items.Length; i++)
			{
				c.Add(items[i]);
			}
			return c;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestGetInternalImplementation()
		{
			Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item> set = NewBigSet();
			BTree bTree = BTree(set);
			Assert.IsNotNull(bTree);
		}

		private Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item> NewBigSet(params BigSetTestCase.Item
			[] initialSet)
		{
			Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item> set = CollectionFactory.ForObjectContainer
				(Db()).NewBigSet<BigSetTestCase.Item>();
			Sharpen.Collections.AddAll(set, Arrays.AsList(initialSet));
			return set;
		}

		/// <exception cref="System.MemberAccessException"></exception>
		public static BTree BTree(Db4objects.Db4o.Collections.ISet<BigSetTestCase.Item> set
			)
		{
			return (BTree)Reflection4.GetFieldValue(set, "_bTree");
		}

		private LocalTransaction FileTransaction()
		{
			return ((LocalTransaction)Trans());
		}
	}
}
