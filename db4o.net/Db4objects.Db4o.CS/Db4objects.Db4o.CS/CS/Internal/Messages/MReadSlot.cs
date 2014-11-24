/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public sealed class MReadSlot : MsgD, IMessageWithResponse
	{
		public sealed override ByteArrayBuffer GetByteLoad()
		{
			int address = _payLoad.ReadInt();
			int length = _payLoad.Length() - (Const4.IntLength);
			Slot slot = new Slot(address, length);
			_payLoad.RemoveFirstBytes(Const4.IntLength);
			_payLoad.UseSlot(slot);
			return this._payLoad;
		}

		public sealed override MsgD GetWriter(StatefulBuffer bytes)
		{
			MsgD message = GetWriterForLength(bytes.Transaction(), bytes.Length() + Const4.IntLength
				);
			message._payLoad.WriteInt(bytes.GetAddress());
			message._payLoad.Append(bytes._buffer);
			return message;
		}

		public Msg ReplyFromServer()
		{
			int address = ReadInt();
			int length = ReadInt();
			lock (ContainerLock())
			{
				StatefulBuffer bytes = new StatefulBuffer(this.Transaction(), address, length);
				try
				{
					Container().ReadBytes(bytes._buffer, address, length);
					return GetWriter(bytes);
				}
				catch (Exception)
				{
					// TODO: not nicely handled on the client side yet
					return Msg.Null;
				}
			}
		}
	}
}
