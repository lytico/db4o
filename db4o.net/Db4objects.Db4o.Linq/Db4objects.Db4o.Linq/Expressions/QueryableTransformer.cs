/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

#if !CF_3_5

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Db4objects.Db4o.Linq.Internals;

namespace Db4objects.Db4o.Linq.Expressions
{
	internal class QueryableTransformer : ExpressionTransformer
	{
		private bool _optimize = true;

		private bool Optimize
		{
			get { return _optimize; }
			set { _optimize = value; }
		}

		public static Expression Transform(Expression expression)
		{
			return new QueryableTransformer().Visit(expression);
		}

		protected override Expression VisitLambda(LambdaExpression lambda)
		{
			return lambda;
		}

		protected override Expression VisitMethodCall(MethodCallExpression call)
		{
			if (IsQueryableExtensionMethod(call.Method))
			{
				return ReplaceQueryableMethodCall(call);
			}

			return base.VisitMethodCall(call);
		}

		protected override Expression VisitConstant (ConstantExpression constant)
		{
			var queryable = constant.Value as IDb4oLinqQueryable;
			if (queryable == null) return constant;

			return Expression.Constant(queryable.GetQuery());
		}

		private static bool IsQueryableExtensionMethod(MethodInfo method)
		{
			return method.IsExtension()
				&& typeof(IQueryable).IsAssignableFrom(method.GetParameters().First().ParameterType);
		}

		private MethodCallExpression ReplaceQueryableMethodCall(MethodCallExpression call)
		{
			var target = null as Expression;
			if (call.Object != null) target = Visit(call.Object);

			var arguments = VisitExpressionList(call.Arguments);
			var method = ReplaceQueryableMethod(call.Method);

			return Expression.Call(target, method, ProcessArguments(method, arguments));
		}

		private IEnumerable<Expression> ProcessArguments(MethodInfo method, ReadOnlyCollection<Expression> arguments)
		{
			var parameters = method.GetParameters();

			for (int i = 0; i < parameters.Length; i++)
				yield return TryUnquoteExpression(arguments[i], parameters[i].ParameterType);
		}

		private static Expression TryUnquoteExpression(Expression expression, Type delegateType)
		{
			if (expression.NodeType != ExpressionType.Quote) return expression;

			var lambda = (LambdaExpression)((UnaryExpression)expression).Operand;
			if (lambda.Type == delegateType) return lambda;

			return expression;
		}

		private MethodInfo ReplaceQueryableMethod(MethodInfo method)
		{
			MethodInfo match;

			if (Optimize)
			{
				if (TryMatchMethod(typeof(Db4oLinqQueryExtensions), method, out match)) return match;
			}

			if (TryMatchMethod(typeof(Enumerable), method, out match))
			{
				if (Optimize) Optimize = false;
				return match;
			}

			throw new ArgumentException(string.Format("Failed to find a replacement for {0}", method));
		}

		private static bool TryMatchMethod(Type target, MethodInfo method, out MethodInfo match)
		{
			foreach (var candidate in target.GetMethods())
			{
				if (TryMatchMethod(method, candidate, out match)) return true;
			}

			match = null;
			return false;
		}

		private static bool TryMatchMethod(MethodInfo method, MethodInfo candidate, out MethodInfo match)
		{
			match = null;
			if (candidate.Name != method.Name) return false;
			if (!candidate.IsExtension()) return false;
			if (!LengthMatch(method.GetParameters(), candidate.GetParameters())) return false;
			if (!TryMatchGenericMethod(method, ref candidate)) return false;
			if (!TryMatchMethodSignature(method, candidate)) return false;

			match = candidate;
			return true;
		}

		private static bool TryMatchGenericMethod(MethodInfo method, ref MethodInfo candidate)
		{
			if (method.IsGenericMethod)
			{
				if (!candidate.IsGenericMethod) return false;
				if (!LengthMatch(candidate.GetGenericArguments(), method.GetGenericArguments())) return false;

				candidate = candidate.MakeGenericMethodFrom(method);
			}

			return true;
		}

		private static bool TryMatchMethodSignature(MethodInfo method, MethodInfo candidate)
		{
			var parameters = method.GetParameterTypes();
			var candidateParameters = candidate.GetParameterTypes();
			var compare = GetTypeComparer(candidate.DeclaringType);

			if (!compare(method.ReturnType, candidate.ReturnType)) return false;

			for (int i = 0; i < candidateParameters.Length; i++)
			{
				if (!compare(parameters[i], candidateParameters[i])) return false;
			}

			return true;
		}

		private static bool LengthMatch<T1, T2>(T1[] a, T2[] b)
		{
			return a.Length == b.Length;
		}

		private static Func<Type, Type, bool> GetTypeComparer(Type type)
		{
			Func<Type, Type> mapper;
			if (!_mappers.TryGetValue(type, out mapper)) mapper = t => t;

			return (a, b) =>
			{
				if (a == b) return true;
				if (mapper(a) == b) return true;
				return mapper(UnboxExpression(a)) == b;
			};
		}

	    private static Type UnboxExpression(Type type)
	    {
	        if(type.IsGenericInstanceOf((typeof(Expression<>))))
	        {
	            return type.GetGenericArguments()[0];
	        }
	        return type;
	    }

	    private static Dictionary<Type, Func<Type, Type>> _mappers = new Dictionary<Type, Func<Type, Type>> {
			{ typeof(Db4oLinqQueryExtensions), MapQueryableToDb4o },
			{ typeof(Enumerable), MapQueryableToEnumerable },
		};

		private static Type MapQueryableToDb4o(Type type)
		{
			if (type.IsGenericInstanceOf(typeof(IQueryable<>)) || type.IsGenericInstanceOf(typeof(IOrderedQueryable<>)))
			{
				type = typeof(IDb4oLinqQuery<>).MakeGenericTypeFrom(type);
			}

			return type;
		}

		private static Type MapQueryableToEnumerable(Type type)
		{
			if (type.IsGenericInstanceOf(typeof(IQueryable<>)))
			{
				type = typeof(IEnumerable<>).MakeGenericTypeFrom(type);
			}
			else if (type.IsGenericInstanceOf(typeof(IOrderedQueryable<>)))
			{
				type = typeof(IOrderedEnumerable<>).MakeGenericTypeFrom(type);
			}
			else if (type.IsGenericInstanceOf(typeof(Expression<>)))
			{
				type = type.GetFirstGenericArgument();
			}
			else if (type == typeof(IQueryable))
			{
				type = typeof(IEnumerable);
			}

			return type;
		}
	}
}

#endif