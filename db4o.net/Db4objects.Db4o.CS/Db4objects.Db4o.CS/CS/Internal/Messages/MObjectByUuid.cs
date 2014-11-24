/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public class MObjectByUuid : MsgD, IMessageWithResponse
	{
		public Msg ReplyFromServer()
		{
			long uuid = ReadLong();
			byte[] signature = ReadBytes();
			int id = 0;
			Transaction trans = Transaction();
			lock (ContainerLock())
			{
				try
				{
					HardObjectReference hardRef = Container().GetHardReferenceBySignature(trans, uuid
						, signature);
					if (hardRef._reference != null)
					{
						id = hardRef._reference.GetID();
					}
				}
				catch (Exception e)
				{
				}
			}
			return Msg.ObjectByUuid.GetWriterForInt(trans, id);
		}
	}
}
