/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.CS.Internal.Messages;

namespace Db4objects.Db4o.CS.Internal
{
	public class MessageEventArgs : EventArgs
	{
		private IMessage _message;

		public MessageEventArgs(IMessage message)
		{
			_message = message;
		}

		public virtual IMessage Message
		{
			get
			{
				return _message;
			}
		}
	}
}
