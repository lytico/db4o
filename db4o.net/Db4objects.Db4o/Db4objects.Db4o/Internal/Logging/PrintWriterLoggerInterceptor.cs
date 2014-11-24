/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Logging;
using Sharpen.IO;

namespace Db4objects.Db4o.Internal.Logging
{
	public class PrintWriterLoggerInterceptor : ILoggingInterceptor
	{
		private PrintWriter @out;

		public PrintWriterLoggerInterceptor(PrintWriter @out)
		{
			this.@out = @out;
		}

		public virtual void Log(Level loggingLevel, string method, object[] args)
		{
			IList throwables = TranslateArguments(args);
			@out.Println(FormatLine(Platform4.Now(), loggingLevel, method, args));
			if (throwables != null)
			{
				for (IEnumerator tIter = throwables.GetEnumerator(); tIter.MoveNext(); )
				{
					Exception t = ((Exception)tIter.Current);
					Platform4.PrintStackTrace(t, @out);
				}
			}
		}

		private IList TranslateArguments(object[] args)
		{
			IList throwables = null;
			if (args == null)
			{
				return null;
			}
			for (int i = 0; i < args.Length; i++)
			{
				object obj = args[i];
				if (obj is Exception)
				{
					Exception t = (Exception)obj;
					args[i] = t.GetType().Name;
					if (throwables == null)
					{
						throwables = new ArrayList();
					}
					throwables.Add(t);
				}
			}
			return throwables;
		}

		public static string FormatLine(DateTime now, Level loggingLevel, string method, 
			object[] args)
		{
			return Platform4.Format(now, true) + " " + FormatMessage(loggingLevel, method, args
				);
		}

		public static string FormatMessage(Level loggingLevel, string method, object[] args
			)
		{
			string s = string.Empty;
			if (args != null)
			{
				for (int objIndex = 0; objIndex < args.Length; ++objIndex)
				{
					object obj = args[objIndex];
					if (s.Length > 0)
					{
						s += ", ";
					}
					s += obj;
				}
			}
			return "[" + Logger.LevelToString(loggingLevel) + "] " + FormatMethodName(method)
				 + (args == null ? string.Empty : "(" + s + ")");
		}

		private static string FormatMethodName(string name)
		{
			return name;
		}
	}
}
