using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;

namespace OME.Logging.ExceptionLogging
{
    #region Interface ILogger
    interface ILogger
    {
        void Write(string message);
    }
    #endregion

    public class FlatFileLogger : ILogger
    {
        #region Private Constant Member Variables
        private const string CONS_ERROR_CATEGORY = "Error";
        #endregion

        #region Private Readonly Variables
        private readonly LogWriter m_logWriter;
        #endregion

        #region Constructor
        public FlatFileLogger(FileStream fStream)
        {
            m_logWriter = CreateFlatFileLogWriter(fStream);
        }
        #endregion

        #region Private Helper Function
        private LogWriter CreateFlatFileLogWriter(FileStream logFile)
        {
            // This is our message template for any Sink you add below in our case the Windows Event Log
            TextFormatter formatter = new TextFormatter("Timestamp: {timestamp}{newline}" +
                                                        "Message: {message}{newline}" +
                                                        "Machine: {machine}{newline}{newline}{newline}");
            
            LogSource emptyTraceSource = new LogSource("none");
            LogSource errorsTraceSource = new LogSource(CONS_ERROR_CATEGORY, System.Diagnostics.SourceLevels.All);

            // Create for all Errors a Listener which writes the messages to the Windows Event Log
            // with the Event Log Source Property "Code Source". The message format is specified by
            // the TextFormatter which is in our case the template above.
            errorsTraceSource.Listeners.Add(new FlatFileTraceListener(logFile, formatter));
            IDictionary<string, LogSource> traceSources = new Dictionary<string, LogSource>();

            // Add to Category "Error" our EventLog Listener with the corresponding category in it.
            traceSources.Add(errorsTraceSource.Name, errorsTraceSource);

            return new LogWriter(new ILogFilter[0], // ICollection<ILogFilter> filters
                              traceSources,        // IDictionary<string, LogSource> traceSources
                              emptyTraceSource,    // LogSource allEventsTraceSource
                              emptyTraceSource,    // LogSource notProcessedTraceSource
                              errorsTraceSource,    // LogSource errorsTraceSource
                              CONS_ERROR_CATEGORY,        // string defaultCategory
                              false,                // bool tracingEnabled
                              true);                // bool logWarningsWhenNoCategoriesMatch
        }
        #endregion

        #region ILogger Members
        public void Write(string message)
        {
            LogEntry ent = new LogEntry();
            ent.TimeStamp = ent.TimeStamp.ToLocalTime();
            ent.Categories.Add(CONS_ERROR_CATEGORY);  // To use another category use traceSources.Add("OtherCat", Listener);
            ent.Message = message;
            ent.Severity = System.Diagnostics.TraceEventType.Error;
            m_logWriter.Write(ent);

        }
        #endregion
    }
}
