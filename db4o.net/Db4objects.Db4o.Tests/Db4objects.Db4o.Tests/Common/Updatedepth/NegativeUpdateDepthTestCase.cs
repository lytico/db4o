/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Updatedepth;

namespace Db4objects.Db4o.Tests.Common.Updatedepth
{
	public class NegativeUpdateDepthTestCase : ITestCase
	{
		public class Item
		{
		}

		public virtual void TestNegativeUpdateDepthIsIllegal()
		{
			ICommonConfiguration config = Db4oEmbedded.NewConfiguration().Common;
			Assert.Expect(typeof(ArgumentException), new _ICodeBlock_17(config));
		}

		private sealed class _ICodeBlock_17 : ICodeBlock
		{
			public _ICodeBlock_17(ICommonConfiguration config)
			{
				this.config = config;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				config.ObjectClass(typeof(NegativeUpdateDepthTestCase.Item)).UpdateDepth(Const4.Unspecified
					);
			}

			private readonly ICommonConfiguration config;
		}
	}
}
