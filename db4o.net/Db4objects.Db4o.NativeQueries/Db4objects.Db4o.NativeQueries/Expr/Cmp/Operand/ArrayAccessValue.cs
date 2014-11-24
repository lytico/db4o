/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Instrumentation.Api;
using Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand;

namespace Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand
{
	public class ArrayAccessValue : ComparisonOperandDescendant
	{
		private IComparisonOperand _index;

		public ArrayAccessValue(ComparisonOperandDescendant parent, IComparisonOperand index
			) : base(parent)
		{
			_index = index;
		}

		public override void Accept(IComparisonOperandVisitor visitor)
		{
			visitor.Visit(this);
		}

		public virtual IComparisonOperand Index()
		{
			return _index;
		}

		public override bool Equals(object obj)
		{
			if (!base.Equals(obj))
			{
				return false;
			}
			Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand.ArrayAccessValue casted = (Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand.ArrayAccessValue
				)obj;
			return _index.Equals(casted._index);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode() * 29 + _index.GetHashCode();
		}

		public override string ToString()
		{
			return base.ToString() + "[" + _index + "]";
		}

		public override ITypeRef Type
		{
			get
			{
				return ((ComparisonOperandDescendant)Parent()).Type.ElementType;
			}
		}
	}
}
