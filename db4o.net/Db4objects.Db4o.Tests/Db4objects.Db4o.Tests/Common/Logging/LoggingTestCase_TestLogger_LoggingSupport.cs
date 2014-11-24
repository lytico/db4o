/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Internal.Logging;
using Db4objects.Db4o.Tests.Common.Logging;

namespace Db4objects.Db4o.Tests.Common.Logging
{
	public class LoggingTestCase_TestLogger_LoggingSupport
	{
		public class LoggingTestCase_TestLoggerAdapter : LoggingTestCase.ITestLogger
		{
			public virtual void Msg()
			{
			}
		}

		public class LoggingTestCase_TestLoggerLogger : LoggingTestCase.ITestLogger
		{
			private LoggingWrapper wrapper;

			private Level level;

			public LoggingTestCase_TestLoggerLogger(LoggingWrapper wrapper, Level level)
			{
				this.wrapper = wrapper;
				this.level = level;
			}

			private void Log(string methodName, object[] args)
			{
				wrapper.Log(level, methodName, args);
			}

			public virtual void Msg()
			{
				LoggingTestCase.ITestLogger forward = ((LoggingTestCase.ITestLogger)wrapper.Forward
					());
				if (forward != null)
				{
					wrapper.PushCurrentLevel(level);
					try
					{
						forward.Msg();
					}
					catch (Exception _exceptionThrown)
					{
						wrapper.ExceptionCaughtInForward("msg", new object[] {  }, _exceptionThrown);
					}
					finally
					{
						wrapper.PopCurrentLevel();
					}
					return;
				}
				Log("msg", new object[] {  });
			}
		}
	}
}
