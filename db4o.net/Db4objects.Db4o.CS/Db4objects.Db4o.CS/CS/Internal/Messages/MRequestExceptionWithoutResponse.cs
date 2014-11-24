/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public class MRequestExceptionWithoutResponse : MsgD, IServerSideMessage
	{
		public virtual void ProcessAtServer()
		{
			Platform4.ThrowUncheckedException((Exception)ReadSingleObject());
		}
	}
}
