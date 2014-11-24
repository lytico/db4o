/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Instrumentation.Api;
using Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand;

namespace Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand
{
	public class StaticFieldRoot : ComparisonOperandRoot
	{
		private ITypeRef _type;

		public StaticFieldRoot(ITypeRef type)
		{
			if (null == type)
			{
				throw new ArgumentNullException();
			}
			_type = type;
		}

		public virtual ITypeRef Type
		{
			get
			{
				return _type;
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand.StaticFieldRoot casted = (Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand.StaticFieldRoot
				)obj;
			return _type.Equals(casted._type);
		}

		public override int GetHashCode()
		{
			return _type.GetHashCode();
		}

		public override string ToString()
		{
			return _type.ToString();
		}

		public override void Accept(IComparisonOperandVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
