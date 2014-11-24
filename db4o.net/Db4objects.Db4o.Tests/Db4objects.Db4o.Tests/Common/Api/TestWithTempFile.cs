/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Foundation.IO;

namespace Db4objects.Db4o.Tests.Common.Api
{
	public partial class TestWithTempFile : ITestLifeCycle
	{
		private string _tempFile;

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
			File4.Delete(TempFile());
		}
	}
}
