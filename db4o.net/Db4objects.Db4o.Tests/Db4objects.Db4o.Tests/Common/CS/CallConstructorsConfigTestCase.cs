/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class CallConstructorsConfigTestCase : StandaloneCSTestCaseBase
	{
		/// <exception cref="System.Exception"></exception>
		protected override void RunTest()
		{
			WithClient(new _IContainerBlock_15());
			WithClient(new _IContainerBlock_21());
		}

		private sealed class _IContainerBlock_15 : IContainerBlock
		{
			public _IContainerBlock_15()
			{
			}

			public void Run(IObjectContainer client)
			{
				client.Store(new StandaloneCSTestCaseBase.Item());
			}
		}

		private sealed class _IContainerBlock_21 : IContainerBlock
		{
			public _IContainerBlock_21()
			{
			}

			public void Run(IObjectContainer client)
			{
				Assert.AreEqual(1, client.Query(typeof(StandaloneCSTestCaseBase.Item)).Count);
			}
		}

		protected override void Configure(IConfiguration config)
		{
			config.CallConstructors(true);
			config.ExceptionsOnNotStorable(true);
		}
	}
}
#endif // !SILVERLIGHT
