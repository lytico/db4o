/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Drs.Foundation;

namespace Db4objects.Drs.Foundation
{
	/// <summary>Experiment field for future db4o logging.</summary>
	/// <remarks>
	/// Experiment field for future db4o logging.
	/// This will become an interface in the future.
	/// It will also allow wrapping to Log4j
	/// For now we are just collecting requirments.
	/// We are not using log4j directly on purpose, so
	/// we can keep the footprint small for embedded
	/// devices
	/// </remarks>
	public class Logger4Support
	{
		private static readonly Logger4 _logger = new Logger4();

		public static void LogIdentity(object obj, string message)
		{
			_logger.LogIdentity(obj, message);
		}

		public static void LogIdentity(object obj)
		{
			LogIdentity(obj, string.Empty);
		}

		public static void Log(string str)
		{
			_logger.Log(str);
		}
	}
}
