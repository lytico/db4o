/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Fieldindex;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Fieldindex;

namespace Db4objects.Db4o.Tests.Common.Fieldindex
{
	public class FieldIndexProcessorTestCase : FieldIndexProcessorTestCaseBase
	{
		public static void Main(string[] args)
		{
			new FieldIndexProcessorTestCase().RunAll();
		}

		protected override void Configure(IConfiguration config)
		{
			base.Configure(config);
			IndexField(config, typeof(NonIndexedFieldIndexItem), "indexed");
		}

		protected override void Store()
		{
			Container().ProduceClassMetadata(ReflectClass(typeof(NonIndexedFieldIndexItem)));
			StoreItems(new int[] { 3, 4, 7, 9 });
			StoreComplexItems(new int[] { 3, 4, 7, 9 }, new int[] { 2, 2, 8, 8 });
		}

		public virtual void TestIdentity()
		{
			IQuery query = CreateComplexItemQuery();
			query.Descend("foo").Constrain(3);
			ComplexFieldIndexItem item = (ComplexFieldIndexItem)query.Execute().Next();
			query = CreateComplexItemQuery();
			query.Descend("child").Constrain(item).Identity();
			AssertExpectedFoos(typeof(ComplexFieldIndexItem), new int[] { 4 }, query);
		}

		public virtual void TestSingleIndexNotSmaller()
		{
			IQuery query = CreateItemQuery();
			query.Descend("foo").Constrain(5).Smaller().Not();
			AssertExpectedFoos(typeof(FieldIndexItem), new int[] { 7, 9 }, query);
		}

		public virtual void TestSingleIndexNotGreater()
		{
			IQuery query = CreateItemQuery();
			query.Descend("foo").Constrain(4).Greater().Not();
			AssertExpectedFoos(typeof(FieldIndexItem), new int[] { 3, 4 }, query);
		}

		public virtual void TestSingleIndexSmallerOrEqual()
		{
			IQuery query = CreateItemQuery();
			query.Descend("foo").Constrain(7).Smaller().Equal();
			AssertExpectedFoos(typeof(FieldIndexItem), new int[] { 3, 4, 7 }, query);
		}

		public virtual void TestSingleIndexGreaterOrEqual()
		{
			IQuery query = CreateItemQuery();
			query.Descend("foo").Constrain(7).Greater().Equal();
			AssertExpectedFoos(typeof(FieldIndexItem), new int[] { 7, 9 }, query);
		}

		public virtual void TestSingleIndexRange()
		{
			IQuery query = CreateItemQuery();
			query.Descend("foo").Constrain(3).Greater();
			query.Descend("foo").Constrain(9).Smaller();
			AssertExpectedFoos(typeof(FieldIndexItem), new int[] { 4, 7 }, query);
		}

		public virtual void TestSingleIndexAndRange()
		{
			IQuery query = CreateItemQuery();
			IConstraint c1 = query.Descend("foo").Constrain(3).Greater();
			IConstraint c2 = query.Descend("foo").Constrain(9).Smaller();
			c1.And(c2);
			AssertExpectedFoos(typeof(FieldIndexItem), new int[] { 4, 7 }, query);
		}

		public virtual void TestSingleIndexOr()
		{
			IQuery query = CreateItemQuery();
			IConstraint c1 = query.Descend("foo").Constrain(4).Smaller();
			IConstraint c2 = query.Descend("foo").Constrain(7).Greater();
			c1.Or(c2);
			AssertExpectedFoos(typeof(FieldIndexItem), new int[] { 3, 9 }, query);
		}

		public virtual void TestExplicitAndOverOr()
		{
			AssertAndOverOrQuery(true);
		}

		public virtual void TestImplicitAndOverOr()
		{
			AssertAndOverOrQuery(false);
		}

		public virtual void TestSingleIndexOrRange()
		{
			IQuery query = CreateItemQuery();
			IConstraint c1 = query.Descend("foo").Constrain(1).Greater();
			IConstraint c2 = query.Descend("foo").Constrain(4).Smaller();
			IConstraint c3 = query.Descend("foo").Constrain(4).Greater();
			IConstraint c4 = query.Descend("foo").Constrain(10).Smaller();
			IConstraint cc1 = c1.And(c2);
			IConstraint cc2 = c3.And(c4);
			cc1.Or(cc2);
			AssertExpectedFoos(typeof(FieldIndexItem), new int[] { 3, 7, 9 }, query);
		}

		public virtual void TestImplicitAndOnOrs()
		{
			IQuery query = CreateItemQuery();
			IConstraint c1 = query.Descend("foo").Constrain(4).Smaller();
			IConstraint c2 = query.Descend("foo").Constrain(3).Greater();
			IConstraint c3 = query.Descend("foo").Constrain(4).Greater();
			c1.Or(c2);
			c1.Or(c3);
			AssertExpectedFoos(typeof(FieldIndexItem), new int[] { 3, 4, 7, 9 }, query);
		}

		public virtual void TestTwoLevelDescendOr()
		{
			IQuery query = CreateComplexItemQuery();
			IConstraint c1 = query.Descend("child").Descend("foo").Constrain(4).Smaller();
			IConstraint c2 = query.Descend("child").Descend("foo").Constrain(4).Greater();
			c1.Or(c2);
			AssertExpectedFoos(typeof(ComplexFieldIndexItem), new int[] { 4, 9 }, query);
		}

		public virtual void TestThreeOrs()
		{
			IQuery query = CreateItemQuery();
			IConstraint c1 = query.Descend("foo").Constrain(3);
			IConstraint c2 = query.Descend("foo").Constrain(4);
			IConstraint c3 = query.Descend("foo").Constrain(7);
			c1.Or(c2).Or(c3);
			AssertExpectedFoos(typeof(FieldIndexItem), new int[] { 3, 4, 7 }, query);
		}

		public virtual void _testOrOnDifferentFields()
		{
			IQuery query = CreateComplexItemQuery();
			IConstraint c1 = query.Descend("foo").Constrain(3);
			IConstraint c2 = query.Descend("bar").Constrain(8);
			c1.Or(c2);
			AssertExpectedFoos(typeof(ComplexFieldIndexItem), new int[] { 3, 7, 9 }, query);
		}

		public virtual void TestCantOptimizeOrInvolvingNonIndexedField()
		{
			IQuery query = CreateQuery(typeof(NonIndexedFieldIndexItem));
			IConstraint c1 = query.Descend("indexed").Constrain(1);
			IConstraint c2 = query.Descend("foo").Constrain(2);
			c1.Or(c2);
			AssertCantOptimize(query);
		}

		public virtual void TestCantOptimizeDifferentLevels()
		{
			IQuery query = CreateComplexItemQuery();
			IConstraint c1 = query.Descend("child").Descend("foo").Constrain(4).Smaller();
			IConstraint c2 = query.Descend("foo").Constrain(7).Greater();
			c1.Or(c2);
			AssertCantOptimize(query);
		}

		public virtual void TestCantOptimizeJoinOnNonIndexedFields()
		{
			IQuery query = CreateQuery(typeof(NonIndexedFieldIndexItem));
			IConstraint c1 = query.Descend("foo").Constrain(1);
			IConstraint c2 = query.Descend("foo").Constrain(2);
			c1.Or(c2);
			AssertCantOptimize(query);
		}

		public virtual void TestIndexSelection()
		{
			IQuery query = CreateComplexItemQuery();
			query.Descend("bar").Constrain(2);
			query.Descend("foo").Constrain(3);
			AssertBestIndex("foo", query);
			query = CreateComplexItemQuery();
			query.Descend("foo").Constrain(3);
			query.Descend("bar").Constrain(2);
			AssertBestIndex("foo", query);
		}

		public virtual void TestDoubleDescendingOnQuery()
		{
			IQuery query = CreateComplexItemQuery();
			query.Descend("child").Descend("foo").Constrain(3);
			AssertExpectedFoos(typeof(ComplexFieldIndexItem), new int[] { 4 }, query);
		}

		public virtual void TestTripleDescendingOnQuery()
		{
			IQuery query = CreateComplexItemQuery();
			query.Descend("child").Descend("child").Descend("foo").Constrain(3);
			AssertExpectedFoos(typeof(ComplexFieldIndexItem), new int[] { 7 }, query);
		}

		public virtual void TestMultiTransactionSmallerWithCommit()
		{
			Transaction transaction = NewTransaction();
			FillTransactionWith(transaction, 0);
			int[] expectedZeros = NewBTreeNodeSizedArray(0);
			AssertSmaller(transaction, expectedZeros, 3);
			transaction.Commit();
			FillTransactionWith(transaction, 5);
			AssertSmaller(IntArrays4.Concat(expectedZeros, new int[] { 3, 4 }), 7);
		}

		public virtual void TestMultiTransactionWithRollback()
		{
			Transaction transaction = NewTransaction();
			FillTransactionWith(transaction, 0);
			int[] expectedZeros = NewBTreeNodeSizedArray(0);
			AssertSmaller(transaction, expectedZeros, 3);
			transaction.Rollback();
			AssertSmaller(transaction, new int[0], 3);
			FillTransactionWith(transaction, 5);
			AssertSmaller(new int[] { 3, 4 }, 7);
		}

		public virtual void TestMultiTransactionSmaller()
		{
			Transaction transaction = NewTransaction();
			FillTransactionWith(transaction, 0);
			int[] expected = NewBTreeNodeSizedArray(0);
			AssertSmaller(transaction, expected, 3);
			FillTransactionWith(transaction, 5);
			AssertSmaller(new int[] { 3, 4 }, 7);
		}

		public virtual void TestMultiTransactionGreater()
		{
			FillTransactionWith(SystemTrans(), 10);
			FillTransactionWith(SystemTrans(), 5);
			AssertGreater(new int[] { 4, 7, 9 }, 3);
			RemoveFromTransaction(SystemTrans(), 5);
			AssertGreater(new int[] { 4, 7, 9 }, 3);
			RemoveFromTransaction(SystemTrans(), 10);
			AssertGreater(new int[] { 4, 7, 9 }, 3);
		}

		public virtual void TestSingleIndexEquals()
		{
			int expectedBar = 3;
			AssertExpectedFoos(typeof(FieldIndexItem), new int[] { expectedBar }, CreateQuery
				(expectedBar));
		}

		public virtual void TestSingleIndexSmaller()
		{
			AssertSmaller(new int[] { 3, 4 }, 7);
		}

		public virtual void TestSingleIndexGreater()
		{
			AssertGreater(new int[] { 4, 7, 9 }, 3);
		}

		private void AssertCantOptimize(IQuery query)
		{
			FieldIndexProcessorResult result = ExecuteProcessor(query);
			Assert.AreSame(FieldIndexProcessorResult.NoIndexFound, result);
		}

		private void AssertBestIndex(string expectedFieldIndex, IQuery query)
		{
			IIndexedNode node = SelectBestIndex(query);
			AssertComplexItemIndex(expectedFieldIndex, node);
		}

		private void AssertAndOverOrQuery(bool explicitAnd)
		{
			IQuery query = CreateItemQuery();
			IConstraint c1 = query.Descend("foo").Constrain(3);
			IConstraint c2 = query.Descend("foo").Constrain(9);
			IConstraint c3 = query.Descend("foo").Constrain(3);
			IConstraint c4 = query.Descend("foo").Constrain(7);
			IConstraint cc1 = c1.Or(c2);
			IConstraint cc2 = c3.Or(c4);
			if (explicitAnd)
			{
				cc1.And(cc2);
			}
			AssertExpectedFoos(typeof(FieldIndexItem), new int[] { 3 }, query);
		}

		private void AssertGreater(int[] expectedFoos, int greaterThan)
		{
			IQuery query = CreateItemQuery();
			query.Descend("foo").Constrain(greaterThan).Greater();
			AssertExpectedFoos(typeof(FieldIndexItem), expectedFoos, query);
		}

		private void AssertExpectedFoos(Type itemClass, int[] expectedFoos, IQuery query)
		{
			Transaction trans = TransactionFromQuery(query);
			int[] expectedIds = MapToObjectIds(CreateQuery(trans, itemClass), expectedFoos);
			AssertExpectedIDs(expectedIds, query);
		}

		private void AssertExpectedIDs(int[] expectedIds, IQuery query)
		{
			FieldIndexProcessorResult result = ExecuteProcessor(query);
			if (expectedIds.Length == 0)
			{
				Assert.AreSame(FieldIndexProcessorResult.FoundIndexButNoMatch, result);
				return;
			}
			ByRef treeInts = ByRef.NewInstance();
			result.Traverse(new _IIntVisitor_303(treeInts));
			AssertTreeInt(expectedIds, ((TreeInt)treeInts.value));
		}

		private sealed class _IIntVisitor_303 : IIntVisitor
		{
			public _IIntVisitor_303(ByRef treeInts)
			{
				this.treeInts = treeInts;
			}

			public void Visit(int i)
			{
				treeInts.value = ((TreeInt)Tree.Add(((TreeInt)treeInts.value), new TreeInt(i)));
			}

			private readonly ByRef treeInts;
		}

		private FieldIndexProcessorResult ExecuteProcessor(IQuery query)
		{
			return CreateProcessor(query).Run();
		}

		private BTree Btree()
		{
			return FieldIndexBTree(typeof(FieldIndexItem), "foo");
		}

		private void Store(Transaction trans, FieldIndexItem item)
		{
			Container().Store(trans, item);
		}

		private void FillTransactionWith(Transaction trans, int bar)
		{
			for (int i = 0; i < BTreeAssert.FillSize(Btree()); ++i)
			{
				Store(trans, new FieldIndexItem(bar));
			}
		}

		private int[] NewBTreeNodeSizedArray(int value)
		{
			BTree btree = Btree();
			return BTreeAssert.NewBTreeNodeSizedArray(btree, value);
		}

		private void RemoveFromTransaction(Transaction trans, int foo)
		{
			IObjectSet found = CreateItemQuery(trans).Execute();
			while (found.HasNext())
			{
				FieldIndexItem item = (FieldIndexItem)found.Next();
				if (item.foo == foo)
				{
					Container().Delete(trans, item);
				}
			}
		}

		private void AssertSmaller(int[] expectedFoos, int smallerThan)
		{
			AssertSmaller(Trans(), expectedFoos, smallerThan);
		}

		private void AssertSmaller(Transaction transaction, int[] expectedFoos, int smallerThan
			)
		{
			IQuery query = CreateItemQuery(transaction);
			query.Descend("foo").Constrain(smallerThan).Smaller();
			AssertExpectedFoos(typeof(FieldIndexItem), expectedFoos, query);
		}
	}
}
