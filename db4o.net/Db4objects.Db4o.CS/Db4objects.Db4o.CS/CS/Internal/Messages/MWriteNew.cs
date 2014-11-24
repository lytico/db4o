/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public sealed class MWriteNew : MsgObject, IServerSideMessage
	{
		public void ProcessAtServer()
		{
			int classMetadataId = _payLoad.ReadInt();
			Unmarshall(_payLoad._offset);
			lock (ContainerLock())
			{
				ClassMetadata classMetadata = classMetadataId == 0 ? null : LocalContainer().ClassMetadataForID
					(classMetadataId);
				int id = _payLoad.GetID();
				Transaction().IdSystem().PrefetchedIDConsumed(id);
				Slot slot = LocalContainer().AllocateSlotForNewUserObject(Transaction(), id, _payLoad
					.Length());
				_payLoad.Address(slot.Address());
				if (classMetadata != null)
				{
					classMetadata.AddFieldIndices(_payLoad);
				}
				LocalContainer().WriteNew(Transaction(), _payLoad.Pointer(), classMetadata, _payLoad
					);
			}
		}
	}
}
