/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand;

namespace Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand
{
	public abstract class ComparisonOperandRoot : IComparisonOperandAnchor
	{
		public virtual IComparisonOperandAnchor Parent()
		{
			return null;
		}

		public IComparisonOperandAnchor Root()
		{
			return this;
		}

		public abstract void Accept(IComparisonOperandVisitor arg1);
	}
}
