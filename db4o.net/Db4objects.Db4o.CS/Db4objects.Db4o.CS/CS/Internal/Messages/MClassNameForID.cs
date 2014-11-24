/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	/// <summary>get the classname for an internal ID</summary>
	public sealed class MClassNameForID : MsgD, IMessageWithResponse
	{
		public Msg ReplyFromServer()
		{
			int id = _payLoad.ReadInt();
			string name = string.Empty;
			lock (ContainerLock())
			{
				ClassMetadata classMetadata = Container().ClassMetadataForID(id);
				if (classMetadata != null)
				{
					name = classMetadata.GetName();
				}
			}
			return Msg.ClassNameForId.GetWriterForString(Transaction(), name);
		}
	}
}
