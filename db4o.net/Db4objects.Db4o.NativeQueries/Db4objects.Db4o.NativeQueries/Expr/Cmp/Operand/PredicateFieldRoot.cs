/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand;

namespace Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand
{
	public class PredicateFieldRoot : ComparisonOperandRoot
	{
		public static readonly Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand.PredicateFieldRoot
			 Instance = new Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand.PredicateFieldRoot
			();

		private PredicateFieldRoot()
		{
		}

		public override string ToString()
		{
			return "PREDICATE";
		}

		public override void Accept(IComparisonOperandVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
