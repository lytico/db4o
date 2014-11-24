/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Classindex;
using Db4objects.Db4o.Internal.Fieldindex;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Fieldindex;

namespace Db4objects.Db4o.Tests.Common.Fieldindex
{
	public abstract class FieldIndexProcessorTestCaseBase : FieldIndexTestCaseBase
	{
		public FieldIndexProcessorTestCaseBase() : base()
		{
		}

		protected override void Configure(IConfiguration config)
		{
			base.Configure(config);
			IndexField(config, typeof(ComplexFieldIndexItem), "foo");
			IndexField(config, typeof(ComplexFieldIndexItem), "bar");
			IndexField(config, typeof(ComplexFieldIndexItem), "child");
		}

		protected virtual IQuery CreateComplexItemQuery()
		{
			return CreateQuery(typeof(ComplexFieldIndexItem));
		}

		protected virtual IIndexedNode SelectBestIndex(IQuery query)
		{
			FieldIndexProcessor processor = CreateProcessor(query);
			return processor.SelectBestIndex();
		}

		protected virtual FieldIndexProcessor CreateProcessor(IQuery query)
		{
			QCandidates candidates = GetQCandidates(query);
			return new FieldIndexProcessor(candidates);
		}

		private QCandidates GetQCandidates(IQuery query)
		{
			QQueryBase.CreateCandidateCollectionResult result = ((QQuery)query).CreateCandidateCollection
				();
			((QQuery)query).CheckConstraintsEvaluationMode();
			QCandidates candidates = (QCandidates)result.candidateCollection._element;
			return candidates;
		}

		protected virtual void AssertComplexItemIndex(string expectedFieldIndex, IIndexedNode
			 node)
		{
			Assert.AreSame(ComplexItemIndex(expectedFieldIndex), node.GetIndex());
		}

		protected virtual BTree FieldIndexBTree(Type clazz, string fieldName)
		{
			return ClassMetadataFor(clazz).FieldMetadataForName(fieldName).GetIndex(null);
		}

		protected virtual BTree ClassIndexBTree(Type clazz)
		{
			return ((BTreeClassIndexStrategy)ClassMetadataFor(clazz).Index()).Btree();
		}

		private BTree ComplexItemIndex(string fieldName)
		{
			return FieldIndexBTree(typeof(ComplexFieldIndexItem), fieldName);
		}

		protected virtual int[] MapToObjectIds(IQuery itemQuery, int[] foos)
		{
			Transaction trans = TransactionFromQuery(itemQuery);
			int[] lookingFor = IntArrays4.Clone(foos);
			int[] objectIds = new int[foos.Length];
			IObjectSet set = itemQuery.Execute();
			while (set.HasNext())
			{
				IHasFoo item = (IHasFoo)set.Next();
				for (int i = 0; i < lookingFor.Length; i++)
				{
					if (lookingFor[i] == item.GetFoo())
					{
						lookingFor[i] = -1;
						objectIds[i] = (int)((ObjectContainerBase)Db()).GetID(trans, item);
						break;
					}
				}
			}
			int index = IndexOfNot(lookingFor, -1);
			if (-1 != index)
			{
				throw new ArgumentException("Foo '" + lookingFor[index] + "' not found!");
			}
			return objectIds;
		}

		public static int IndexOfNot(int[] array, int value)
		{
			for (int i = 0; i < array.Length; ++i)
			{
				if (value != array[i])
				{
					return i;
				}
			}
			return -1;
		}

		protected virtual void StoreComplexItems(int[] foos, int[] bars)
		{
			ComplexFieldIndexItem last = null;
			for (int i = 0; i < foos.Length; i++)
			{
				last = new ComplexFieldIndexItem(foos[i], bars[i], last);
				Store(last);
			}
		}

		protected virtual void AssertTreeInt(int[] expectedValues, TreeInt treeInt)
		{
			ExpectingVisitor visitor = ExpectingVisitor.CreateExpectingVisitor(expectedValues
				);
			treeInt.Traverse(new _IVisitor4_113(visitor));
			visitor.AssertExpectations();
		}

		private sealed class _IVisitor4_113 : IVisitor4
		{
			public _IVisitor4_113(ExpectingVisitor visitor)
			{
				this.visitor = visitor;
			}

			public void Visit(object obj)
			{
				visitor.Visit(((TreeInt)obj)._key);
			}

			private readonly ExpectingVisitor visitor;
		}

		protected virtual Transaction TransactionFromQuery(IQuery query)
		{
			return ((QQuery)query).Transaction();
		}
	}
}
