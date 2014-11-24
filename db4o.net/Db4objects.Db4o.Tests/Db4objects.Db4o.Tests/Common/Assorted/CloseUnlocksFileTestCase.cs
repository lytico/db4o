/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.Tests.Common.Api;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class CloseUnlocksFileTestCase : Db4oTestWithTempFile
	{
		[System.ObsoleteAttribute]
		public virtual void Test()
		{
			File4.Delete(TempFile());
			Assert.IsFalse(Exists(TempFile()));
			IObjectContainer oc = Db4oEmbedded.OpenFile(NewConfiguration(), TempFile());
			oc.Close();
			File4.Delete(TempFile());
			Assert.IsFalse(Exists(TempFile()));
		}

		private bool Exists(string fileName)
		{
			return new Sharpen.IO.File(fileName).Exists();
		}
	}
}
