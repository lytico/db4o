/* Copyright (C) 2010 Versant Inc.  http://www.db4o.com */
#if SILVERLIGHT

using Db4objects.Db4o.Config;
using Db4objects.Db4o.IO;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.Silverlight.Config
{
	public class SilverlightSupportTestCase : ITestCase
	{
		public void Test()
		{
			var config = Db4oEmbedded.NewConfiguration();
			config.AddConfigurationItem(new SilverlightSupport());

			using (Db4oEmbedded.OpenFile(config, "SilverlightSupportTestCase.odb"))
			{
			}

			Assert.AreEqual(typeof(IsolatedStorageStorage), config.File.Storage.GetType());
		}
	}
}

#endif