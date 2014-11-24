/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Common.Api;

namespace Db4objects.Db4o.Tests.Common.Api
{
	public class Db4oTestWithTempFile : TestWithTempFile, IDb4oTestCase
	{
		protected virtual IEmbeddedConfiguration NewConfiguration()
		{
			return Db4oEmbedded.NewConfiguration();
		}
	}
}
#endif // !SILVERLIGHT
