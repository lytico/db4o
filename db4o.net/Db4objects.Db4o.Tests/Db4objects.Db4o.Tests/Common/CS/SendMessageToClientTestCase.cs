/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class SendMessageToClientTestCase : ClientServerTestCaseBase
	{
		public static void Main(string[] args)
		{
			new SendMessageToClientTestCase().RunNetworking();
		}

		public virtual void Test()
		{
			if (IsEmbedded())
			{
				// No sending messages back and forth on MTOC.
				return;
			}
			ServerDispatcher().Write(Msg.Ok);
			Msg msg = Client().GetResponse();
			Assert.AreEqual(Msg.Ok, msg);
		}
	}
}
#endif // !SILVERLIGHT
