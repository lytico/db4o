/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Messaging;
using Db4objects.Db4o.Tests.Common.CS;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class ServerToClientTestCase : MessagingTestCaseBase
	{
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(ServerToClientTestCase)).Run();
		}

		internal sealed class AutoReplyRecipient : IMessageRecipient
		{
			public void ProcessMessage(IMessageContext context, object message)
			{
				IMessageSender sender = context.Sender;
				sender.Send("reply: " + message);
			}
		}

		internal interface IClientWaitLogic
		{
			void Wait(IObjectContainer client1, IObjectContainer client2);
		}

		public virtual void TestInterleavedCommits()
		{
			AssertReplyBehavior(new _IClientWaitLogic_30());
		}

		private sealed class _IClientWaitLogic_30 : ServerToClientTestCase.IClientWaitLogic
		{
			public _IClientWaitLogic_30()
			{
			}

			public void Wait(IObjectContainer client1, IObjectContainer client2)
			{
				client2.Commit();
				client1.Commit();
			}
		}

		private void AssertReplyBehavior(ServerToClientTestCase.IClientWaitLogic clientWaitLogic
			)
		{
			MessagingTestCaseBase.MessageCollector collector1 = new MessagingTestCaseBase.MessageCollector
				();
			MessagingTestCaseBase.MessageCollector collector2 = new MessagingTestCaseBase.MessageCollector
				();
			IObjectServer server = OpenServerWith(new ServerToClientTestCase.AutoReplyRecipient
				());
			try
			{
				IObjectContainer client1 = OpenClient("client1", server);
				try
				{
					SetMessageRecipient(client1, collector1);
					IMessageSender sender1 = MessageSender(client1);
					IObjectContainer client2 = OpenClient("client2", server);
					try
					{
						SetMessageRecipient(client2, collector2);
						IMessageSender sender2 = MessageSender(client2);
						SendEvenOddMessages(sender1, sender2);
						clientWaitLogic.Wait(client1, client2);
						try
						{
							// Give the message processor thread time to dispatch the message.
							Thread.Sleep(100);
						}
						catch (Exception e)
						{
							Sharpen.Runtime.PrintStackTrace(e);
						}
						Assert.AreEqual("[reply: 0, reply: 2, reply: 4, reply: 6, reply: 8]", collector1.
							messages.ToString());
						Assert.AreEqual("[reply: 1, reply: 3, reply: 5, reply: 7, reply: 9]", collector2.
							messages.ToString());
					}
					finally
					{
						client2.Close();
					}
				}
				finally
				{
					client1.Close();
				}
			}
			finally
			{
				server.Close();
			}
		}

		private void SendEvenOddMessages(IMessageSender even, IMessageSender odd)
		{
			for (int i = 0; i < 10; ++i)
			{
				int message = i;
				if (i % 2 == 0)
				{
					even.Send(message);
				}
				else
				{
					odd.Send(message);
				}
			}
		}
	}
}
#endif // !SILVERLIGHT
