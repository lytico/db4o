/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Jre5.Collections.Typehandler;

namespace Db4objects.Db4o.Tests.Jre5.Collections.Typehandler
{
	public abstract class CollectionTypeHandlerUnitTest : TypeHandlerTestUnitBase
	{
		protected abstract void AssertCompareItems(object element, bool successful);

		protected virtual void AssertQuery(bool successful, object element, bool withContains
			)
		{
			IQuery q = NewQuery(ItemFactory().ItemClass());
			IConstraint constraint = q.Descend(ItemFactory().FieldName()).Constrain(element);
			if (withContains)
			{
				constraint.Contains();
			}
			AssertQueryResult(q, successful);
		}

		public virtual void TestRetrieveInstance()
		{
			object item = RetrieveItemInstance();
			AssertContent(item);
		}

		protected virtual object RetrieveItemInstance()
		{
			Type itemClass = ItemFactory().ItemClass();
			object item = RetrieveOnlyInstance(itemClass);
			return item;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestDefragRetrieveInstance()
		{
			Defragment();
			object item = RetrieveItemInstance();
			AssertContent(item);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestSuccessfulQuery()
		{
			AssertQuery(true, Elements()[0], false);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestFailingQuery()
		{
			AssertQuery(false, NotContained(), false);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestSuccessfulContainsQuery()
		{
			AssertQuery(true, Elements()[0], true);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestFailingContainsQuery()
		{
			AssertQuery(false, NotContained(), true);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestCompareItems()
		{
			AssertCompareItems(Elements()[0], true);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestFailingCompareItems()
		{
			AssertCompareItems(NotContained(), false);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestDeletion()
		{
			AssertReferenceTypeElementCount(Elements().Length);
			object item = RetrieveOnlyInstance(ItemFactory().ItemClass());
			Db().Delete(item);
			Db().Purge();
			Db4oAssert.PersistedCount(0, ItemFactory().ItemClass());
			AssertReferenceTypeElementCount(0);
		}

		public virtual void TestJoin()
		{
			IQuery q = NewQuery(ItemFactory().ItemClass());
			q.Descend(ItemFactory().FieldName()).Constrain(Elements()[0]).And(q.Descend(ItemFactory
				().FieldName()).Constrain(Elements()[1]));
			AssertQueryResult(q, true);
		}

		public virtual void TestSubQuery()
		{
			IQuery q = NewQuery(ItemFactory().ItemClass());
			IQuery qq = q.Descend(ItemFactory().FieldName());
			qq.Constrain(Elements()[0]);
			IObjectSet set = qq.Execute();
			Assert.AreEqual(1, set.Count);
			AssertPlainContent(set.Next());
		}

		protected virtual void AssertReferenceTypeElementCount(int expected)
		{
			if (!IsReferenceElement(ElementClass()))
			{
				return;
			}
			Db4oAssert.PersistedCount(expected, ElementClass());
		}

		private bool IsReferenceElement(Type elementClass)
		{
			return typeof(ListTypeHandlerTestVariables.ReferenceElement) == elementClass;
		}
	}
}
