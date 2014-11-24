/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand;

namespace Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand
{
	public class CandidateFieldRoot : ComparisonOperandRoot
	{
		public static readonly Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand.CandidateFieldRoot
			 Instance = new Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand.CandidateFieldRoot
			();

		private CandidateFieldRoot()
		{
		}

		public override string ToString()
		{
			return "CANDIDATE";
		}

		public override void Accept(IComparisonOperandVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
