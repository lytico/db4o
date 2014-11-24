/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public class MReadBytes : MsgD
	{
		public virtual Msg GetWriter(Transaction trans, ByteArrayBuffer bytes)
		{
			MsgD msg = GetWriterForLength(trans, bytes.Length());
			msg._payLoad.Append(bytes._buffer);
			return msg;
		}

		public ByteArrayBuffer Unmarshall()
		{
			if (_payLoad._buffer.Length == 0)
			{
				return null;
			}
			return new ByteArrayBuffer(_payLoad._buffer);
		}
	}
}
