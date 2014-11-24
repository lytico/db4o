/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.CS.Internal.Messages;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public class MRuntimeException : MsgD
	{
		public virtual void ThrowPayload()
		{
			throw ((Exception)ReadSingleObject());
		}
	}
}
