/* Copyright (C) 2009 Versant Inc.  http://www.db4o.com */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Db4oUnit;

namespace Db4objects.Db4o.Linq.Tests
{
	public class CollectionContainsOptimizationTestCase : AbstractDb4oLinqTestCase
	{


#if !CF //csc fails to find S.R.FieldInfo.GetFieldFromHandle


        protected override void Db4oSetupAfterStore()
        {
            Container().ProduceClassMetadata(ReflectClass(typeof(ListHolder)));
        }


		public class CollectionHolder<T> where T : ICollection<string>
		{
			public T Items;
		}

		public class ListHolder : CollectionHolder<List<string>>
		{
		}

		public class IListOfTHolder : CollectionHolder<IList<string>>
		{
		}

		public class ICollectionHolder : CollectionHolder<ICollection<string>>
		{
		}

		public void TestQueryOnIListContains()
		{
			AssertQuery(
				db => from IListOfTHolder p in db
					  where p.Items.Contains("foo")
					  select p);
		}

		public void TestQueryOnIListNotContains()
		{
			AssertQuery(
				db => from IListOfTHolder p in db
					  where !p.Items.Contains("foo")
					  select p);
		}

		public void TestQueryOnListContains()
		{
			AssertQuery(
				db => from ListHolder p in db
					  where p.Items.Contains("foo")
					  select p);
		}

		public void TestQueryOnListNotContains()
		{
			AssertQuery(
				db => from ListHolder p in db
					  where !p.Items.Contains("foo")
					  select p);
		}

		public void TestQueryOnICollectionContains()
		{
			AssertQuery(
				db => from ICollectionHolder p in db
					  where p.Items.Contains("foo")
					  select p);
		}

		public void TestQueryOnICollectionNotContains()
		{
			AssertQuery(
				db => from ICollectionHolder p in db
					  where !p.Items.Contains("foo")
					  select p);
		}

		private void AssertQuery<T>(Expression<Func<IObjectContainer, IEnumerable<T>>> queryExpression)
		{
			using (var recorder = new QueryStringRecorder(Db()))
			{
				var result = queryExpression.Compile().Invoke(Db());
				result.ToList();

				string expected = ExpectedRepresentationFor(queryExpression);
				Assert.AreEqual(expected, recorder.QueryString);
			}
		}

		private static string ExpectedRepresentationFor<T>(Expression<Func<IObjectContainer, IEnumerable<T>>> queryExpression)
		{
			return string.Format("({0}(Items {1} 'foo'))", ExtentTypeFrom(queryExpression), ContainmentCondition(queryExpression));
		}

		private static string ContainmentCondition<T>(Expression<Func<IObjectContainer, IEnumerable<T>>> queryExpression)
		{
			Assert.AreEqual(ExpressionType.Call, queryExpression.Body.NodeType);

			MethodCallExpression whereMethod = FindMethodCall(queryExpression, "Where");

			var whereExpression = ((LambdaExpression)((UnaryExpression)whereMethod.Arguments[1]).Operand);

			if (whereExpression.Body.NodeType == ExpressionType.Not)
			{
				return "not";
			}

			Assert.AreEqual(ExpressionType.Call, whereExpression.Body.NodeType);

			var whereCondition = (MethodCallExpression) whereExpression.Body;
			return whereCondition.Method.Name.ToLower();
		}

		private static string ExtentTypeFrom<T>(Expression<Func<IObjectContainer, IEnumerable<T>>> queryExpression)
		{
			MethodCallExpression castMethod = FindMethodCall(queryExpression, "Cast");
			Assert.IsNotNull(castMethod);
			return castMethod.Method.GetGenericArguments()[0].Name;
		}

		private static MethodCallExpression FindMethodCall<T>(Expression<Func<IObjectContainer, IEnumerable<T>>> queryExpression, string methodName)
		{
			Expression expression = queryExpression.Body;
			while (expression.NodeType == ExpressionType.Call)
			{
				var mce = (MethodCallExpression)expression;
				if (mce.Method.Name == methodName)
				{
					return mce;
				}
				expression = mce.Arguments[0];
			}

			return null;
		}
#endif
	}
}
