/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Messaging;
using Db4objects.Db4o.Tests.Common.Staging;

namespace Db4objects.Db4o.Tests.Common.Staging
{
	public class PingTestCase : Db4oClientServerTestCase, IOptOutAllButNetworkingCS
	{
		public static void Main(string[] args)
		{
			new PingTestCase().RunAll();
		}

		protected override void Configure(IConfiguration config)
		{
			config.ClientServer().TimeoutClientSocket(1000);
		}

		internal PingTestCase.TestMessageRecipient recipient = new PingTestCase.TestMessageRecipient
			();

		public virtual void Test()
		{
			ClientServerFixture().Server().Ext().Configure().ClientServer().SetMessageRecipient
				(recipient);
			IExtObjectContainer client = ClientServerFixture().Db();
			IMessageSender sender = client.Configure().ClientServer().GetMessageSender();
			if (IsEmbedded())
			{
				Assert.Expect(typeof(NotSupportedException), new _ICodeBlock_36(sender));
				return;
			}
			sender.Send(new PingTestCase.Data());
			// The following query will be block by the sender
			IObjectSet os = client.QueryByExample(null);
			while (os.HasNext())
			{
				os.Next();
			}
			Assert.IsFalse(client.IsClosed());
		}

		private sealed class _ICodeBlock_36 : ICodeBlock
		{
			public _ICodeBlock_36(IMessageSender sender)
			{
				this.sender = sender;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				sender.Send(new PingTestCase.Data());
			}

			private readonly IMessageSender sender;
		}

		public class TestMessageRecipient : IMessageRecipient
		{
			public virtual void ProcessMessage(IMessageContext con, object message)
			{
				Runtime4.Sleep(3000);
			}
		}

		public class Data
		{
		}
	}
}
