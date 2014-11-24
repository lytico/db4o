/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Qlin;
using Db4objects.Db4o.Qlin;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Internal.Qlin
{
	/// <exclude></exclude>
	public class QLinOrderBy : QLinSubNode
	{
		private readonly IQuery _node;

		public QLinOrderBy(QLinRoot root, object expression, QLinOrderByDirection direction
			) : base(root)
		{
			_node = root.Descend(expression);
			if (direction == QLinSupport.Ascending())
			{
				_node.OrderAscending();
			}
			else
			{
				_node.OrderDescending();
			}
		}
	}
}
