/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Internal.Qlin;
using Db4objects.Db4o.Qlin;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Internal.Qlin
{
	/// <exclude></exclude>
	public abstract class QLinSubNode : QLinSodaNode
	{
		protected readonly QLinRoot _root;

		public QLinSubNode(QLinRoot root)
		{
			_root = root;
		}

		protected override QLinRoot Root()
		{
			return _root;
		}

		protected virtual IQuery Query()
		{
			return Root().Query();
		}

		public override IQLin Limit(int size)
		{
			Root().Limit(size);
			return this;
		}

		public override IObjectSet Select()
		{
			return Root().Select();
		}
	}
}
