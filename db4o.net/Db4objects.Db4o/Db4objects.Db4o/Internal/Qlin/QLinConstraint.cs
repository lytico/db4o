/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Qlin;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Internal.Qlin
{
	/// <exclude></exclude>
	public class QLinConstraint : QLinSubNode
	{
		private readonly IConstraint _constraint;

		public QLinConstraint(QLinRoot root, IConstraint constraint) : base(root)
		{
			_constraint = constraint;
		}
	}
}
