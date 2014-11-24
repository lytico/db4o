/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
#if SILVERLIGHT
using Db4objects.Db4o.Config;

namespace Db4objects.Db4o.Tests.Common.Api
{
	public class Db4oTestWithTempFile : TestWithTempFile
	{
		protected virtual IEmbeddedConfiguration NewConfiguration()
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			config.AddConfigurationItem(new SilverlightSupport());
			return config;
		}
	}
}
#endif