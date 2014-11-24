/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections.Generic;
using System.Linq;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Foundation;

using Db4oUnit;
using Db4oUnit.Extensions;

using Db4objects.Db4o.Linq.Tests.Queries;

namespace Db4objects.Db4o.Linq.Tests
{
	public abstract class AbstractDb4oLinqTestCase : AbstractDb4oTestCase
	{
#if SILVERLIGHT && NET_3_5
		public static void AssertSet<T>(IEnumerable<T> expected, IEnumerable<T> actual)
		{
			var message = string.Format("Expected {0}, got {1}", Iterators.ToString(expected), Iterators.ToString(actual));
			Assert.AreEqual(expected.Count(), actual.Count(), message);
            Iterator4Assert.SameContent(expected.GetEnumerator(), actual.GetEnumerator());
		}
#else
		public static void AssertSet<T>(IEnumerable<T> expected, IEnumerable<T> actual)
		{
            var expectedSet = new HashSet<T>(expected);
			var actualSet = new HashSet<T>(actual);

			var message = string.Format("Expected {0}, got {1}", Iterators.ToString(expectedSet), Iterators.ToString(actualSet));
			Assert.AreEqual(expectedSet.Count, actualSet.Count, message);
			Assert.IsTrue(expectedSet.SetEquals(actualSet), message);
		}
#endif

        public static void AssertSequence<T>(IEnumerable<T> expected, IEnumerable<T> candidate)
		{
			Iterator4Assert.AreEqual(expected.GetEnumerator(), candidate.GetEnumerator());
		}

		protected void AssertQuery(string expected, Action action)
		{
			using (var recorder = new QueryStringRecorder(Db()))
			{
				action();

				Assert.AreEqual(expected, recorder.QueryString);
			}
		}

		protected class QueryStringRecorder : IDisposable
		{
			private string _queryString;
			private IEventRegistry _registry;

			public string QueryString
			{
				get { return string.IsNullOrEmpty(_queryString) ? null : _queryString; }
			}

			public QueryStringRecorder(IObjectContainer container)
			{
				_registry = EventRegistryFactory.ForObjectContainer(container);
				_registry.QueryStarted += OnQueryStarted;
			}

			private void OnQueryStarted(object sender, QueryEventArgs args)
			{
				_queryString = args.Query.ToQueryString();
			}

			public void Dispose()
			{
				_registry.QueryStarted -= OnQueryStarted;
			}
		}

		protected void AssertQuery<T>(IDb4oLinqQuery<T> query, string expectedQuery, IEnumerable<T> expectedSet)
		{
			AssertQuery(query, expectedQuery, actualSet => AssertSet(expectedSet, actualSet));
		}
		
		protected void AssertQuery<T>(IEnumerable<T> query, string expectedQuery, Action<IEnumerable<T>> expectation)
		{
			using (var recorder = new QueryStringRecorder(Db()))
			{
				List<T> actualSet = query.ToList();
				Assert.AreEqual(expectedQuery, recorder.QueryString);
				expectation(actualSet);
			}
		}
	}
}
