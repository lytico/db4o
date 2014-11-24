/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Fieldindex;

namespace Db4objects.Db4o.Tests.Common.Fieldindex
{
	public class FieldIndexTestCase : FieldIndexTestCaseBase
	{
		private static readonly int[] Foos = new int[] { 3, 7, 9, 4 };

		public static void Main(string[] arguments)
		{
			new FieldIndexTestCase().RunSolo();
		}

		protected override void Configure(IConfiguration config)
		{
			base.Configure(config);
		}

		protected override void Store()
		{
			StoreItems(Foos);
		}

		public virtual void TestTraverseValues()
		{
			IStoredField field = StoredField();
			ExpectingVisitor expectingVisitor = new ExpectingVisitor(IntArrays4.ToObjectArray
				(Foos));
			field.TraverseValues(expectingVisitor);
			expectingVisitor.AssertExpectations();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestAllThere()
		{
			for (int i = 0; i < Foos.Length; i++)
			{
				IQuery q = CreateQuery(Foos[i]);
				IObjectSet objectSet = q.Execute();
				Assert.AreEqual(1, objectSet.Count);
				FieldIndexItem fii = (FieldIndexItem)objectSet.Next();
				Assert.AreEqual(Foos[i], fii.foo);
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestAccessingBTree()
		{
			BTree bTree = StoredField().GetIndex(Trans());
			Assert.IsNotNull(bTree);
			ExpectKeysSearch(bTree, Foos);
		}

		private void ExpectKeysSearch(BTree btree, int[] values)
		{
			int lastValue = int.MinValue;
			for (int i = 0; i < values.Length; i++)
			{
				if (values[i] != lastValue)
				{
					ExpectingVisitor expectingVisitor = ExpectingVisitor.CreateExpectingVisitor(values
						[i], IntArrays4.Occurences(values, values[i]));
					IBTreeRange range = FieldIndexKeySearch(Trans(), btree, values[i]);
					BTreeAssert.TraverseKeys(range, new _IVisitor4_62(expectingVisitor));
					expectingVisitor.AssertExpectations();
					lastValue = values[i];
				}
			}
		}

		private sealed class _IVisitor4_62 : IVisitor4
		{
			public _IVisitor4_62(ExpectingVisitor expectingVisitor)
			{
				this.expectingVisitor = expectingVisitor;
			}

			public void Visit(object obj)
			{
				IFieldIndexKey fik = (IFieldIndexKey)obj;
				expectingVisitor.Visit(fik.Value());
			}

			private readonly ExpectingVisitor expectingVisitor;
		}

		private IFieldIndexKey FieldIndexKey(int integerPart, object composite)
		{
			return new FieldIndexKeyImpl(integerPart, composite);
		}

		private IBTreeRange FieldIndexKeySearch(Transaction trans, BTree btree, object key
			)
		{
			// SearchTarget should not make a difference, HIGHEST is faster
			BTreeNodeSearchResult start = btree.SearchLeafByObject(trans, FieldIndexKey(0, key
				), SearchTarget.Lowest);
			BTreeNodeSearchResult end = btree.SearchLeafByObject(trans, FieldIndexKey(int.MaxValue
				, key), SearchTarget.Lowest);
			return start.CreateIncludingRange(end);
		}

		private FieldMetadata StoredField()
		{
			return ClassMetadataFor(typeof(FieldIndexItem)).FieldMetadataForName("foo");
		}
	}
}
