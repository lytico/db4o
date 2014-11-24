/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Instrumentation.Api;
using Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand;

namespace Db4objects.Db4o.NativeQueries.Instrumentation
{
	internal class TypeDeducingVisitor : IComparisonOperandVisitor
	{
		private ITypeRef _predicateClass;

		private ITypeRef _clazz;

		private IReferenceProvider _referenceProvider;

		public TypeDeducingVisitor(IReferenceProvider provider, ITypeRef predicateClass)
		{
			this._predicateClass = predicateClass;
			this._referenceProvider = provider;
			_clazz = null;
		}

		public virtual void Visit(PredicateFieldRoot root)
		{
			_clazz = _predicateClass;
		}

		public virtual void Visit(CandidateFieldRoot root)
		{
		}

		//		_clazz=_candidateClass;
		public virtual void Visit(StaticFieldRoot root)
		{
			_clazz = root.Type;
		}

		public virtual ITypeRef OperandClass()
		{
			return _clazz;
		}

		public virtual void Visit(ArithmeticExpression operand)
		{
		}

		public virtual void Visit(ConstValue operand)
		{
			_clazz = _referenceProvider.ForType(operand.Value().GetType());
		}

		public virtual void Visit(FieldValue operand)
		{
			_clazz = operand.Field.Type;
		}

		public virtual void Visit(ArrayAccessValue operand)
		{
			operand.Parent().Accept(this);
			_clazz = _clazz.ElementType;
		}

		public virtual void Visit(MethodCallValue operand)
		{
			_clazz = operand.Method.ReturnType;
		}
	}
}
