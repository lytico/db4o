/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public sealed class MWriteUpdate : MsgObject, IServerSideMessage
	{
		public void ProcessAtServer()
		{
			int classMetadataID = _payLoad.ReadInt();
			int arrayTypeValue = _payLoad.ReadInt();
			ArrayType arrayType = ArrayType.ForValue(arrayTypeValue);
			Unmarshall(_payLoad._offset);
			lock (ContainerLock())
			{
				ClassMetadata classMetadata = LocalContainer().ClassMetadataForID(classMetadataID
					);
				int id = _payLoad.GetID();
				Transaction().DontDelete(id);
				Slot clientSlot = _payLoad.Slot();
				Slot newSlot = null;
				if (clientSlot.IsUpdate())
				{
					Transaction().WriteUpdateAdjustIndexes(id, classMetadata, arrayType);
					newSlot = LocalContainer().AllocateSlotForUserObjectUpdate(_payLoad.Transaction()
						, _payLoad.GetID(), _payLoad.Length());
				}
				else
				{
					if (clientSlot.IsNew())
					{
						// Just one known usecase for this one: For updating plain objects from old versions, since
						// they didnt't have own slots that could be freed.
						// Logic that got us here in OpenTypeHandler7#addReference()#writeUpdate()
						newSlot = LocalContainer().AllocateSlotForNewUserObject(_payLoad.Transaction(), _payLoad
							.GetID(), _payLoad.Length());
					}
					else
					{
						throw new InvalidOperationException();
					}
				}
				_payLoad.Address(newSlot.Address());
				classMetadata.AddFieldIndices(_payLoad);
				_payLoad.WriteEncrypt();
				DeactivateCacheFor(id);
			}
		}

		private void DeactivateCacheFor(int id)
		{
			ObjectReference reference = Transaction().ReferenceForId(id);
			if (null == reference)
			{
				return;
			}
			reference.Deactivate(Transaction(), new FixedActivationDepth(1));
		}
	}
}
