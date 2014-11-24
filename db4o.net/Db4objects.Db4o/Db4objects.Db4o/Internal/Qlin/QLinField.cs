/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Qlin;
using Db4objects.Db4o.Qlin;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Internal.Qlin
{
	/// <exclude></exclude>
	public class QLinField : QLinSubNode
	{
		private readonly IQuery _node;

		public QLinField(QLinRoot root, object expression) : base(root)
		{
			_node = root.Descend(expression);
		}

		public override IQLin Equal(object obj)
		{
			IConstraint constraint = _node.Constrain(obj);
			constraint.Equal();
			return new QLinConstraint(((QLinRoot)_root), constraint);
		}

		public override IQLin StartsWith(string @string)
		{
			IConstraint constraint = _node.Constrain(@string);
			constraint.StartsWith(true);
			return new QLinConstraint(((QLinRoot)_root), constraint);
		}

		public override IQLin Smaller(object obj)
		{
			IConstraint constraint = _node.Constrain(obj);
			constraint.Smaller();
			return new QLinConstraint(((QLinRoot)_root), constraint);
		}

		public override IQLin Greater(object obj)
		{
			IConstraint constraint = _node.Constrain(obj);
			constraint.Greater();
			return new QLinConstraint(((QLinRoot)_root), constraint);
		}
	}
}
