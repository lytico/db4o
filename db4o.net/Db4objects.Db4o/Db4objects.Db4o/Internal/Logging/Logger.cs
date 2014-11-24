/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Internal.Logging;
using Sharpen.IO;
using Sharpen.Lang;

namespace Db4objects.Db4o.Internal.Logging
{
	public class Logger
	{
		internal static ThreadLocal currentThreadLoggingLevel = new ThreadLocal();

		private static IDictionary cache = new Hashtable();

		public static readonly Level Trace = new Level("TRACE", 0);

		public static readonly Level Debug = new Level("DEBUG", 1);

		public static readonly Level Info = new Level("INFO", 2);

		public static readonly Level Warn = new Level("WARN", 3);

		public static readonly Level Error = new Level("ERROR", 4);

		public static readonly Level Fatal = new Level("FATAL", 5);

		internal static ILoggingInterceptor rootInterceptor = new PrintWriterLoggerInterceptor
			(new PrintWriter(Sharpen.Runtime.Out, true));

		internal static Level loggingLevel = Trace;

		public static void Intercept(ILoggingInterceptor interceptor)
		{
			Logger.rootInterceptor = interceptor;
		}

		public static ILogging Get(Type clazz)
		{
			lock (cache)
			{
				ILogging logging = (ILogging)((ILogging)cache[clazz]);
				if (logging == null)
				{
					logging = new LoggingWrapper(clazz);
					cache[clazz] = logging;
				}
				return logging;
			}
		}

		public static void LoggingLevel(Level loggingLevel)
		{
			Logger.loggingLevel = loggingLevel;
		}

		public static string LevelToString(Level loggingLevel)
		{
			return loggingLevel.ToString();
		}

		public static Level ContextLoggingLevel()
		{
			return ((Level)currentThreadLoggingLevel.Get());
		}

		public static void PurgeCache()
		{
			lock (cache)
			{
				cache.Clear();
			}
		}
	}
}
