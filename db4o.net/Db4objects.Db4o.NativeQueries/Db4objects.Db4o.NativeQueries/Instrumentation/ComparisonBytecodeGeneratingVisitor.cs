/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Instrumentation.Api;
using Db4objects.Db4o.NativeQueries.Expr.Cmp;
using Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand;
using Db4objects.Db4o.NativeQueries.Instrumentation;

namespace Db4objects.Db4o.NativeQueries.Instrumentation
{
	internal class ComparisonBytecodeGeneratingVisitor : IComparisonOperandVisitor
	{
		private IMethodBuilder _methodBuilder;

		private ITypeRef _predicateClass;

		private bool _inArithmetic = false;

		private ITypeRef _opClass = null;

		private ITypeRef _staticRoot = null;

		public ComparisonBytecodeGeneratingVisitor(IMethodBuilder methodBuilder, ITypeRef
			 predicateClass)
		{
			this._methodBuilder = methodBuilder;
			this._predicateClass = predicateClass;
		}

		public virtual void Visit(ConstValue operand)
		{
			object value = operand.Value();
			if (value != null)
			{
				_opClass = TypeRef(value.GetType());
			}
			_methodBuilder.Ldc(value);
			if (value != null)
			{
				Box(_opClass, !_inArithmetic);
			}
		}

		private ITypeRef TypeRef(Type type)
		{
			return _methodBuilder.References.ForType(type);
		}

		public virtual void Visit(FieldValue fieldValue)
		{
			ITypeRef lastFieldClass = fieldValue.Field.Type;
			bool needConversion = lastFieldClass.IsPrimitive;
			fieldValue.Parent().Accept(this);
			if (_staticRoot != null)
			{
				_methodBuilder.LoadStaticField(fieldValue.Field);
				_staticRoot = null;
				return;
			}
			_methodBuilder.LoadField(fieldValue.Field);
			Box(lastFieldClass, !_inArithmetic && needConversion);
		}

		public virtual void Visit(CandidateFieldRoot root)
		{
			_methodBuilder.LoadArgument(1);
		}

		public virtual void Visit(PredicateFieldRoot root)
		{
			_methodBuilder.LoadArgument(0);
		}

		public virtual void Visit(StaticFieldRoot root)
		{
			_staticRoot = root.Type;
		}

		public virtual void Visit(ArrayAccessValue operand)
		{
			ITypeRef cmpType = DeduceFieldClass(operand.Parent()).ElementType;
			operand.Parent().Accept(this);
			bool outerInArithmetic = _inArithmetic;
			_inArithmetic = true;
			operand.Index().Accept(this);
			_inArithmetic = outerInArithmetic;
			_methodBuilder.LoadArrayElement(cmpType);
			Box(cmpType, !_inArithmetic);
		}

		public virtual void Visit(MethodCallValue operand)
		{
			IMethodRef method = operand.Method;
			ITypeRef retType = method.ReturnType;
			// FIXME: this should be handled within conversions
			bool needConversion = retType.IsPrimitive;
			operand.Parent().Accept(this);
			bool oldInArithmetic = _inArithmetic;
			for (int paramIdx = 0; paramIdx < operand.Args.Length; paramIdx++)
			{
				_inArithmetic = operand.Method.ParamTypes[paramIdx].IsPrimitive;
				operand.Args[paramIdx].Accept(this);
			}
			_inArithmetic = oldInArithmetic;
			_methodBuilder.Invoke(method, operand.CallingConvention);
			Box(retType, !_inArithmetic && needConversion);
		}

		public virtual void Visit(ArithmeticExpression operand)
		{
			bool oldInArithmetic = _inArithmetic;
			_inArithmetic = true;
			operand.Left().Accept(this);
			operand.Right().Accept(this);
			ITypeRef operandType = ArithmeticType(operand);
			switch (operand.Op().Id())
			{
				case ArithmeticOperator.AddId:
				{
					_methodBuilder.Add(operandType);
					break;
				}

				case ArithmeticOperator.SubtractId:
				{
					_methodBuilder.Subtract(operandType);
					break;
				}

				case ArithmeticOperator.MultiplyId:
				{
					_methodBuilder.Multiply(operandType);
					break;
				}

				case ArithmeticOperator.DivideId:
				{
					_methodBuilder.Divide(operandType);
					break;
				}

				case ArithmeticOperator.ModuloId:
				{
					_methodBuilder.Modulo(operandType);
					break;
				}

				default:
				{
					throw new Exception("Unknown operand: " + operand.Op());
				}
			}
			Box(_opClass, !oldInArithmetic);
			_inArithmetic = oldInArithmetic;
		}

		// FIXME: need to map dX,fX,...
		private void Box(ITypeRef boxedType, bool canApply)
		{
			if (!canApply)
			{
				return;
			}
			_methodBuilder.Box(boxedType);
		}

		private ITypeRef DeduceFieldClass(IComparisonOperand fieldValue)
		{
			TypeDeducingVisitor visitor = new TypeDeducingVisitor(_methodBuilder.References, 
				_predicateClass);
			fieldValue.Accept(visitor);
			return visitor.OperandClass();
		}

		private ITypeRef ArithmeticType(IComparisonOperand operand)
		{
			if (operand is ConstValue)
			{
				return PrimitiveType(((ConstValue)operand).Value().GetType());
			}
			if (operand is FieldValue)
			{
				return ((FieldValue)operand).Field.Type;
			}
			if (operand is ArithmeticExpression)
			{
				ArithmeticExpression expr = (ArithmeticExpression)operand;
				ITypeRef left = ArithmeticType(expr.Left());
				ITypeRef right = ArithmeticType(expr.Right());
				if (left == DoubleType() || right == DoubleType())
				{
					return DoubleType();
				}
				if (left == FloatType() || right == FloatType())
				{
					return FloatType();
				}
				if (left == LongType() || right == LongType())
				{
					return LongType();
				}
				return IntType();
			}
			return null;
		}

		private ITypeRef PrimitiveType(Type klass)
		{
			if (klass == typeof(int) || klass == typeof(short) || klass == typeof(bool) || klass
				 == typeof(byte))
			{
				return IntType();
			}
			if (klass == typeof(double))
			{
				return DoubleType();
			}
			if (klass == typeof(float))
			{
				return FloatType();
			}
			if (klass == typeof(long))
			{
				return LongType();
			}
			return TypeRef(klass);
		}

		private ITypeRef IntType()
		{
			return TypeRef(typeof(int));
		}

		private ITypeRef LongType()
		{
			return TypeRef(typeof(long));
		}

		private ITypeRef FloatType()
		{
			return TypeRef(typeof(float));
		}

		private ITypeRef DoubleType()
		{
			return TypeRef(typeof(double));
		}
	}
}
