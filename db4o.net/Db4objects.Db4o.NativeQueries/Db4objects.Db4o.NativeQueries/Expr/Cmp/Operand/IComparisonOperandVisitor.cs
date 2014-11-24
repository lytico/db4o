/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand;

namespace Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand
{
	public interface IComparisonOperandVisitor
	{
		void Visit(ArithmeticExpression operand);

		void Visit(ConstValue operand);

		void Visit(FieldValue operand);

		void Visit(CandidateFieldRoot root);

		void Visit(PredicateFieldRoot root);

		void Visit(StaticFieldRoot root);

		void Visit(ArrayAccessValue operand);

		void Visit(MethodCallValue value);
	}
}
