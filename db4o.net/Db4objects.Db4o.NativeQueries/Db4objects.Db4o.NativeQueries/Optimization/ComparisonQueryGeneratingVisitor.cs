/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Reflection;
using Db4objects.Db4o.Instrumentation.Api;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.NativeQueries.Expr.Cmp;
using Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand;
using Db4objects.Db4o.NativeQueries.Optimization;
using Sharpen.Lang.Reflect;

namespace Db4objects.Db4o.NativeQueries.Optimization
{
	internal sealed class ComparisonQueryGeneratingVisitor : IComparisonOperandVisitor
	{
		private object _predicate;

		private object _value = null;

		private readonly INativeClassFactory _classSource;

		private readonly IReferenceResolver _resolver;

		public object Value()
		{
			return _value;
		}

		public void Visit(ConstValue operand)
		{
			_value = operand.Value();
		}

		public void Visit(FieldValue operand)
		{
			operand.Parent().Accept(this);
			Type clazz = ((operand.Parent() is StaticFieldRoot) ? (Type)_value : _value.GetType
				());
			try
			{
				FieldInfo field = Reflection4.GetField(clazz, operand.FieldName());
				_value = field.GetValue(_value);
			}
			catch (Exception exc)
			{
				// arg is ignored for static
				Sharpen.Runtime.PrintStackTrace(exc);
			}
		}

		internal object Add(object a, object b)
		{
			if (a is double || b is double)
			{
				return ((double)a) + ((double)b);
			}
			if (a is float || b is float)
			{
				return ((float)a) + ((float)b);
			}
			if (a is long || b is long)
			{
				return ((long)a) + ((long)b);
			}
			return ((int)a) + ((int)b);
		}

		internal object Subtract(object a, object b)
		{
			if (a is double || b is double)
			{
				return ((double)a) - ((double)b);
			}
			if (a is float || b is float)
			{
				return ((float)a) - ((float)b);
			}
			if (a is long || b is long)
			{
				return ((long)a) - ((long)b);
			}
			return ((int)a) - ((int)b);
		}

		internal object Multiply(object a, object b)
		{
			if (a is double || b is double)
			{
				return ((double)a) * ((double)b);
			}
			if (a is float || b is float)
			{
				return ((float)a) * ((float)b);
			}
			if (a is long || b is long)
			{
				return ((long)a) * ((long)b);
			}
			return ((int)a) * ((int)b);
		}

		internal object Divide(object a, object b)
		{
			if (a is double || b is double)
			{
				return ((double)a) / ((double)b);
			}
			if (a is float || b is float)
			{
				return ((float)a) / ((float)b);
			}
			if (a is long || b is long)
			{
				return ((long)a) / ((long)b);
			}
			return ((int)a) / ((int)b);
		}

		internal object Modulo(object a, object b)
		{
			if (a is double || b is double)
			{
				return ((double)a) % ((double)b);
			}
			if (a is float || b is float)
			{
				return ((float)a) % ((float)b);
			}
			if (a is long || b is long)
			{
				return ((long)a) % ((long)b);
			}
			return ((int)a) % ((int)b);
		}

		public void Visit(ArithmeticExpression operand)
		{
			operand.Left().Accept(this);
			object left = _value;
			operand.Right().Accept(this);
			object right = _value;
			switch (operand.Op().Id())
			{
				case ArithmeticOperator.AddId:
				{
					_value = Add(left, right);
					break;
				}

				case ArithmeticOperator.SubtractId:
				{
					_value = Subtract(left, right);
					break;
				}

				case ArithmeticOperator.MultiplyId:
				{
					_value = Multiply(left, right);
					break;
				}

				case ArithmeticOperator.DivideId:
				{
					_value = Divide(left, right);
					break;
				}

				case ArithmeticOperator.ModuloId:
				{
					_value = Modulo(left, right);
					break;
				}
			}
		}

		public void Visit(CandidateFieldRoot root)
		{
		}

		public void Visit(PredicateFieldRoot root)
		{
			_value = _predicate;
		}

		public void Visit(StaticFieldRoot root)
		{
			try
			{
				_value = _classSource.ForName(root.Type.Name);
			}
			catch (TypeLoadException e)
			{
				Sharpen.Runtime.PrintStackTrace(e);
			}
		}

		public void Visit(ArrayAccessValue operand)
		{
			operand.Parent().Accept(this);
			object parent = _value;
			operand.Index().Accept(this);
			int index = (int)_value;
			_value = Sharpen.Runtime.GetArrayValue(parent, index);
		}

		public void Visit(MethodCallValue operand)
		{
			operand.Parent().Accept(this);
			object receiver = _value;
			MethodInfo method = _resolver.Resolve(operand.Method);
			try
			{
				_value = method.Invoke(IsStatic(method) ? null : receiver, Args(operand));
			}
			catch (Exception exc)
			{
				Sharpen.Runtime.PrintStackTrace(exc);
				_value = null;
			}
		}

		private object[] Args(MethodCallValue operand)
		{
			IComparisonOperand[] args = operand.Args;
			object[] @params = new object[args.Length];
			for (int paramIdx = 0; paramIdx < args.Length; paramIdx++)
			{
				args[paramIdx].Accept(this);
				@params[paramIdx] = _value;
			}
			return @params;
		}

		private bool IsStatic(MethodInfo method)
		{
			return NativeQueriesPlatform.IsStatic(method);
		}

		public ComparisonQueryGeneratingVisitor(object predicate, INativeClassFactory classSource
			, IReferenceResolver resolver) : base()
		{
			_predicate = predicate;
			_classSource = classSource;
			_resolver = resolver;
		}
	}
}
