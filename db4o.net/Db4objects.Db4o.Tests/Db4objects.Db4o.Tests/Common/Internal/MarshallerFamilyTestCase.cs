/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal.Marshall;

namespace Db4objects.Db4o.Tests.Common.Internal
{
	public class MarshallerFamilyTestCase : ITestCase
	{
		public virtual void TestThrowingOnNewerVersion()
		{
			Assert.Expect(typeof(IncompatibleFileFormatException), new _ICodeBlock_13());
		}

		private sealed class _ICodeBlock_13 : ICodeBlock
		{
			public _ICodeBlock_13()
			{
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				MarshallerFamily.Version(int.MaxValue);
			}
		}
	}
}
