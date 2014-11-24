/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Ids;

namespace Db4objects.Db4o.Internal.Ids
{
	/// <exclude></exclude>
	public interface IStackableIdSystem : IIdSystem
	{
		int ChildId();

		void ChildId(int id);
	}
}
