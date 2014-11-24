/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public class MReadReaderById : MsgD, IMessageWithResponse
	{
		public Msg ReplyFromServer()
		{
			ByteArrayBuffer bytes = null;
			// readWriterByID may fail in certain cases, for instance if
			// and object was deleted by another client
			try
			{
				lock (ContainerLock())
				{
					bytes = Container().ReadBufferById(Transaction(), _payLoad.ReadInt(), _payLoad.ReadInt
						() == 1);
				}
				if (bytes == null)
				{
					bytes = new ByteArrayBuffer(0);
				}
			}
			catch (Db4oRecoverableException exc)
			{
				throw;
			}
			catch (Exception exc)
			{
				throw new Db4oRecoverableException(exc);
			}
			return Msg.ReadBytes.GetWriter(Transaction(), bytes);
		}
	}
}
