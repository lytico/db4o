/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Instrumentation.Api;
using Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand;

namespace Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand
{
	public class FieldValue : ComparisonOperandDescendant
	{
		private readonly IFieldRef _field;

		public FieldValue(IComparisonOperandAnchor root, IFieldRef field) : base(root)
		{
			_field = field;
		}

		public virtual string FieldName()
		{
			return _field.Name;
		}

		public override bool Equals(object other)
		{
			if (!base.Equals(other))
			{
				return false;
			}
			Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand.FieldValue casted = (Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand.FieldValue
				)other;
			return _field.Equals(casted._field);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode() * 29 + _field.GetHashCode();
		}

		public override string ToString()
		{
			return base.ToString() + "." + _field;
		}

		public override void Accept(IComparisonOperandVisitor visitor)
		{
			visitor.Visit(this);
		}

		public virtual IFieldRef Field
		{
			get
			{
				return _field;
			}
		}

		public override ITypeRef Type
		{
			get
			{
				return _field.Type;
			}
		}
	}
}
