/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Qlin;
using Db4objects.Db4o.Qlin;

namespace Db4objects.Db4o.Internal.Qlin
{
	/// <exclude></exclude>
	public abstract class QLinSodaNode : QLinNode
	{
		protected abstract QLinRoot Root();

		public override IQLin Where(object expression)
		{
			return new QLinField(Root(), expression);
		}

		public override IQLin OrderBy(object expression, QLinOrderByDirection direction)
		{
			return new QLinOrderBy(Root(), expression, direction);
		}
	}
}
