/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using System.Collections;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Events;

namespace Db4objects.Db4o.Tests.Common.CS
{
	internal class MessageCollector
	{
		public static IList ForServerDispatcher(IServerMessageDispatcher dispatcher)
		{
			ArrayList _messages = new ArrayList();
			dispatcher.MessageReceived += new System.EventHandler<MessageEventArgs>(new _IEventListener4_16
				(_messages).OnEvent);
			return _messages;
		}

		private sealed class _IEventListener4_16
		{
			public _IEventListener4_16(ArrayList _messages)
			{
				this._messages = _messages;
			}

			public void OnEvent(object sender, MessageEventArgs args)
			{
				_messages.Add(((MessageEventArgs)args).Message);
			}

			private readonly ArrayList _messages;
		}
	}
}
#endif // !SILVERLIGHT
