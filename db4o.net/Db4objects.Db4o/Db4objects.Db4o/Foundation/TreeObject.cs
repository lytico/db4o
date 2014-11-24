/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class TreeObject : Tree
	{
		private readonly object _object;

		private readonly IComparison4 _function;

		public TreeObject(object @object, IComparison4 function)
		{
			_object = @object;
			_function = function;
		}

		public override int Compare(Tree tree)
		{
			return _function.Compare(_object, tree.Key());
		}

		public override object Key()
		{
			return _object;
		}
	}
}
