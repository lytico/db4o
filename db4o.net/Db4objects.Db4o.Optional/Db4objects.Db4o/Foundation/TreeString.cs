/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Handlers;

namespace Db4objects.Db4o.Foundation
{
	public class TreeString : Tree
	{
		public string _key;

		public TreeString(string key)
		{
			this._key = key;
		}

		protected override Tree ShallowCloneInternal(Tree tree)
		{
			Db4objects.Db4o.Foundation.TreeString ts = (Db4objects.Db4o.Foundation.TreeString
				)base.ShallowCloneInternal(tree);
			ts._key = _key;
			return ts;
		}

		public override object ShallowClone()
		{
			return ShallowCloneInternal(new Db4objects.Db4o.Foundation.TreeString(_key));
		}

		public override int Compare(Tree to)
		{
			return StringHandler.Compare(Const4.stringIO.Write(_key), Const4.stringIO.Write((
				(Db4objects.Db4o.Foundation.TreeString)to)._key));
		}

		public override object Key()
		{
			return _key;
		}
	}
}
