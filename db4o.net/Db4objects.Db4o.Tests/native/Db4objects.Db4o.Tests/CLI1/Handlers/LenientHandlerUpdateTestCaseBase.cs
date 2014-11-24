/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
	abstract class LenientHandlerUpdateTestCaseBase : HandlerUpdateTestCaseBase
	{
		protected override void ConfigureForTest(IConfiguration config)
		{
			base.ConfigureForTest(config);
			config.ExceptionsOnNotStorable(false);
		}

        protected bool NullableSupported()
        {
            return Db4oHandlerVersion() >= 4;
        }
	}
}
