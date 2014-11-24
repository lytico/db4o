/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public sealed class MGetClasses : MsgD, IMessageWithResponse
	{
		public Msg ReplyFromServer()
		{
			lock (ContainerLock())
			{
				try
				{
					// Since every new Client reads the class
					// collection from the file, we have to 
					// make sure, it has been written.
					Container().ClassCollection().Write(Transaction());
				}
				catch (Exception)
				{
				}
			}
			MsgD message = Msg.GetClasses.GetWriterForLength(Transaction(), Const4.IntLength 
				+ 1);
			ByteArrayBuffer writer = message.PayLoad();
			writer.WriteInt(Container().ClassCollection().GetID());
			writer.WriteByte(Container().StringIO().EncodingByte());
			return message;
		}
	}
}
