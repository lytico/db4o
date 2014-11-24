/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.CS.Internal.Objectexchange
{
	public interface ISlotAccessor
	{
		Slot CurrentSlotOfID(int id);
	}
}
