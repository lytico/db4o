/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Db4objects.Db4o.Tests.Common.Config
{
	public class ConfigurationOfObjectClassNotSupportedTestCase : ITestCase
	{
		public virtual void Test()
		{
			IEmbeddedConfiguration embeddedConfiguration = Db4oEmbedded.NewConfiguration();
			Assert.Expect(typeof(ArgumentException), new _ICodeBlock_14(embeddedConfiguration
				));
		}

		private sealed class _ICodeBlock_14 : ICodeBlock
		{
			public _ICodeBlock_14(IEmbeddedConfiguration embeddedConfiguration)
			{
				this.embeddedConfiguration = embeddedConfiguration;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				embeddedConfiguration.Common.ObjectClass(typeof(object));
			}

			private readonly IEmbeddedConfiguration embeddedConfiguration;
		}
	}
}
