/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Internal.Btree
{
	/// <exclude></exclude>
	public sealed class SearchTarget
	{
		public static readonly Db4objects.Db4o.Internal.Btree.SearchTarget Lowest = new Db4objects.Db4o.Internal.Btree.SearchTarget
			("Lowest");

		public static readonly Db4objects.Db4o.Internal.Btree.SearchTarget Any = new Db4objects.Db4o.Internal.Btree.SearchTarget
			("Any");

		public static readonly Db4objects.Db4o.Internal.Btree.SearchTarget Highest = new 
			Db4objects.Db4o.Internal.Btree.SearchTarget("Highest");

		private readonly string _target;

		public SearchTarget(string target)
		{
			_target = target;
		}

		public override string ToString()
		{
			return _target;
		}
	}
}
