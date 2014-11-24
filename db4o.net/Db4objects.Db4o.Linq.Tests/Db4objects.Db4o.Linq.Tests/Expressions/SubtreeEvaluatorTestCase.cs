/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Db4objects.Db4o.Linq.Expressions;

using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Linq.Tests.Expressions
{
	public class SubtreeEvaluatorTestCase : AbstractDb4oTestCase
	{
		public class Parameter
		{
			public int ID { get; set; }
		}

		public void TestReplaceInlineCode()
		{
			var exp = CreateExpression(p => p.ID == (12 + 30));

			AssertExpression("(p.ID = 42)", SubtreeEvaluator.Evaluate(exp));
		}

		public void TestReplaceLocalVariable()
		{
			var id = 42;
			var exp = CreateExpression(p => p.ID == id);

			AssertExpression("(p.ID = 42)", SubtreeEvaluator.Evaluate(exp));
		}

		private int _id = 42;

		public void TestReplaceInstanceField()
		{
			var exp = CreateExpression(p => p.ID == _id);

			AssertExpression("(p.ID = 42)", SubtreeEvaluator.Evaluate(exp));
		}

		private static int _sid = 42;

		public void TestReplaceClassField()
		{
			var exp = CreateExpression(p => p.ID == _sid);

			AssertExpression("(p.ID = 42)", SubtreeEvaluator.Evaluate(exp));
		}

		public void TestComplexReplace()
		{
			var exp = CreateExpression(p => (p.ID == 42 || p.ID == p.ID + 12 / 3) && p.ID.ToString() == 42.ToString());

			AssertExpression("(((p.ID = 42) || (p.ID = (p.ID + 4))) && (p.ID.ToString() = \"42\"))", SubtreeEvaluator.Evaluate(exp));
		}

		static void AssertExpression(string expected, Expression expression)
		{
#if NET_4_0
			const string EqualsSignNotPreceededByExclamation = "(?<!\\!)\\=";
			expected = Regex.Replace(expected, EqualsSignNotPreceededByExclamation, "==").Replace("||", "OrElse").Replace("&&", "AndAlso");
#endif

			if (expression.NodeType == ExpressionType.Lambda)
			{
				expression = ((LambdaExpression)expression).Body;
			}

			Assert.AreEqual(expected, expression.ToString());
		}

		static Expression CreateExpression<T>(Expression<Func<Parameter, T>> expression)
		{
			return expression;
		}
	}
}
