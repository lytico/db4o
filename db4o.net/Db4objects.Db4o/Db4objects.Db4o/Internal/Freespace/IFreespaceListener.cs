/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Internal.Freespace
{
	/// <exclude></exclude>
	public interface IFreespaceListener
	{
		void SlotAdded(int size);

		void SlotRemoved(int size);
	}
}
