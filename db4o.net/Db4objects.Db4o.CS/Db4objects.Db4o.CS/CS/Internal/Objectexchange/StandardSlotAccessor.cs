/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal.Objectexchange;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.CS.Internal.Objectexchange
{
	public class StandardSlotAccessor : ISlotAccessor
	{
		private LocalTransaction _transaction;

		public StandardSlotAccessor(LocalTransaction transaction)
		{
			_transaction = transaction;
		}

		public virtual Slot CurrentSlotOfID(int id)
		{
			return _transaction.IdSystem().CurrentSlot(id);
		}
	}
}
