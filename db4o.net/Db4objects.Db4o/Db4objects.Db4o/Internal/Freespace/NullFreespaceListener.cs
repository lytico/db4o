/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Freespace;

namespace Db4objects.Db4o.Internal.Freespace
{
	/// <exclude></exclude>
	public class NullFreespaceListener : IFreespaceListener
	{
		public static readonly IFreespaceListener Instance = new Db4objects.Db4o.Internal.Freespace.NullFreespaceListener
			();

		private NullFreespaceListener()
		{
		}

		public virtual void SlotAdded(int size)
		{
		}

		// do nothing;
		public virtual void SlotRemoved(int size)
		{
		}
		// do nothing
	}
}
