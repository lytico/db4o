/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Logging;

namespace Db4objects.Db4o.Internal.Logging
{
	public interface ILoggingInterceptor
	{
		void Log(Level loggingLevel, string method, object[] args);
	}
}
