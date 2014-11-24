/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Jre5.Concurrency.Query;

namespace Db4objects.Db4o.Tests.Jre5.Concurrency.Query
{
	public class ConcurrentLazyQueriesTestCase : Db4oClientServerTestCase
	{
		private const int ItemCount = 100;

		public sealed class Item
		{
			public int id;

			public ConcurrentLazyQueriesTestCase.Item parent;

			public Item()
			{
			}

			public Item(ConcurrentLazyQueriesTestCase.Item parent_, int id_)
			{
				id = id_;
				parent = parent_;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			ConfigLazyQueries(config);
		}

		private void ConfigLazyQueries(IConfiguration config)
		{
			config.Queries().EvaluationMode(QueryEvaluationMode.Lazy);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			ConcurrentLazyQueriesTestCase.Item root = new ConcurrentLazyQueriesTestCase.Item(
				null, -1);
			for (int i = 0; i < ItemCount; ++i)
			{
				Store(new ConcurrentLazyQueriesTestCase.Item(root, i));
			}
		}

		public virtual void Conc(IExtObjectContainer client)
		{
			IExtObjectContainer container = FileSession();
			ConcurrentLazyQueriesTestCase.Item root = QueryRoot(container);
			for (int i = 0; i < 100; ++i)
			{
				AssertAllItems(QueryItems(root, container));
			}
		}

		private ConcurrentLazyQueriesTestCase.Item QueryRoot(IExtObjectContainer container
			)
		{
			IQuery q = ItemQuery(container);
			q.Descend("id").Constrain(-1);
			return (ConcurrentLazyQueriesTestCase.Item)q.Execute().Next();
		}

		private void AssertAllItems(IEnumerator result)
		{
			Collection4 expected = Range(ItemCount);
			for (int i = 0; i < ItemCount; ++i)
			{
				ConcurrentLazyQueriesTestCase.Item nextItem = (ConcurrentLazyQueriesTestCase.Item
					)IteratorPlatform.Next(result);
				expected.Remove(nextItem.id);
			}
			Assert.AreEqual("[]", expected.ToString());
		}

		private Collection4 Range(int end)
		{
			Collection4 range = new Collection4();
			for (int i = 0; i < end; ++i)
			{
				range.Add(i);
			}
			return range;
		}

		private IEnumerator QueryItems(ConcurrentLazyQueriesTestCase.Item parent, IExtObjectContainer
			 container)
		{
			IQuery q = ItemQuery(container);
			q.Descend("parent").Constrain(parent).Identity();
			// the cast is necessary for sharpen
			return ((IEnumerable)q.Execute()).GetEnumerator();
		}

		private IQuery ItemQuery(IExtObjectContainer container)
		{
			return NewQuery(container, typeof(ConcurrentLazyQueriesTestCase.Item));
		}
	}
}
