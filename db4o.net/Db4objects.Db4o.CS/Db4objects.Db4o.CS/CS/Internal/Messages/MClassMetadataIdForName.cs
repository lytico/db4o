/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	/// <exclude></exclude>
	public class MClassMetadataIdForName : MsgD, IMessageWithResponse
	{
		public Msg ReplyFromServer()
		{
			string name = ReadString();
			try
			{
				lock (ContainerLock())
				{
					int id = Container().ClassMetadataIdForName(name);
					return Msg.ClassId.GetWriterForInt(SystemTransaction(), id);
				}
			}
			catch (Db4oException)
			{
			}
			// TODO: send the exception to the client
			return Msg.Failed;
		}
	}
}
