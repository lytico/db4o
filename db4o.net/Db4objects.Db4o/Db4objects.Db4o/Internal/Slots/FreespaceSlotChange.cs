/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal.Slots
{
	/// <exclude></exclude>
	public class FreespaceSlotChange : IdSystemSlotChange
	{
		public FreespaceSlotChange(int id) : base(id)
		{
		}

		protected override bool ForFreespace()
		{
			return true;
		}
	}
}
