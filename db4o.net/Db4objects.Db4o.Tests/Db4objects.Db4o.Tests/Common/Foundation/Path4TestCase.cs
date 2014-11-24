/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.IO;
using Db4oUnit;
using Db4objects.Db4o.Foundation.IO;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	/// <exclude></exclude>
	public class Path4TestCase : ITestCase
	{
		#if !SILVERLIGHT
		public virtual void TestGetTempFileName()
		{
			string tempFileName = Path.GetTempFileName();
			Assert.IsTrue(System.IO.File.Exists(tempFileName));
			File4.Delete(tempFileName);
		}
		#endif // !SILVERLIGHT
	}
}
