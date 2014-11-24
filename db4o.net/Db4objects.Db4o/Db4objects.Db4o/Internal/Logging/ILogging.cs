/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Logging;

namespace Db4objects.Db4o.Internal.Logging
{
	public interface ILogging
	{
		object Trace();

		object Debug();

		object Info();

		object Warn();

		object Error();

		object Fatal();

		void LoggingLevel(Level loggingLevel);

		Level LoggingLevel();

		void Forward(object forward);

		object Forward();
	}
}
