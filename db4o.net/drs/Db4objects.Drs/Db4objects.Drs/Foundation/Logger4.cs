/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Drs.Foundation;
using Sharpen;

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
	public class Logger4
	{
		public virtual void LogIdentity(object obj, string message)
		{
			if (obj == null)
			{
				Log(message + " [null]");
			}
			Log(message + " " + obj.GetType() + " " + Runtime.IdentityHashCode(obj));
		}

		public virtual void Log(string message)
		{
			return;
			Sharpen.Runtime.Out.WriteLine(StackAnalyzer.MethodCallAboveAsString(typeof(Logger4Support
				)) + " " + message);
			Sharpen.Runtime.Out.Flush();
		}
	}
}
