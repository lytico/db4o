using System;
using System.Drawing;

namespace Db4oTestRunner
{
	public interface ILogger
	{
		void LogMessage(string message, params object[] args);
		void LogMessageFormated(Color foreground, Font font, string message, params object[] args);
		void LogError(string message, params object[] args);
		void LogException(Exception ex);
	}
}
