using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using OME.Logging.Common;
using OME.Logging.Tracing;
using OME.Logging.Exceptions;
using OME.Logging.ExceptionLogging;

namespace OME.Logging.Tracing
{
    public static class OMETrace
    {
        #region constants
        private const string START_FUNCTION = "START ";
        private const string END_FUNCTION = "END ";
        #endregion

        private class TraceInfo
        {
            #region Private Constants
            private const bool CONS_TRACING_ENABLED = false;
            private const float CONS_DEFAULT_TRACE_FILE_SIZE = 2.0F;

            #endregion

            #region Private Variables
            private bool m_IsTracingEnabled = CONS_TRACING_ENABLED;
            private float m_FileSizeInMB = CONS_DEFAULT_TRACE_FILE_SIZE;
            #endregion

            #region Public Properties
            public bool IsTracingEnabled
            {
                get { return m_IsTracingEnabled; }
                set { m_IsTracingEnabled = value; }
            }
            public float FileSizeInMB
            {
                get { return m_FileSizeInMB; }
                set { m_FileSizeInMB = value; }
            }
            #endregion
        }

        #region Private Constants
        //private const string CONS_TRACE_FILE_BASE_NAME = "TraceLog<<sequence>>.log";
        //private const string CONS_NODE_TRACING_ENABLED = "TracingEnabled";
        //private const string CONS_NODE_TRACE_FILE_SIZE = "TraceFileSizeInMB";
        //private const int CONS_TOTAL_TRACE_FILES = 3;
        private static readonly object m_traceLock = new object();
        #endregion

        #region Private Static Variables
        private static TraceInfo m_TraceInfo;
        private static bool m_IsTraceFileStreamInitialized = false;
        #endregion

        #region Static Constructor
        #region XML Documentation Static Constructor
        /// <summary>
        /// Static Constructor
        /// </summary>
        #endregion
        static OMETrace()
        {
            m_TraceInfo = GetTraceInfoFromAppConfig();
        }
        #endregion

        #region Public Static Functions

        /// <summary>
        /// Used to initialize the tracing. The function rethrows any exception occured to the caller. 
        /// The caller is required to put this function call in a try block.
        /// </summary>
        public static void Initialize()
        {
            if (!m_TraceInfo.IsTracingEnabled)
            {
                return;
            }
            InitializeTraceFileStream();
        }

        /// <summary>
        /// Function used to start the file stream to which the trace is written.
        /// </summary>
        private static void InitializeTraceFileStream()
        {
            //Trace file stream exists, return.
            if (m_IsTraceFileStreamInitialized)
            {
                return;
            }

            try
            {
                FileStream traceFileStream = null;
                TraceLogFilePolicy traceLogFilePolicy = new TraceLogFilePolicy
                    (LoggingConstants.CONS_TRACE_FILE_BASE_NAME, m_TraceInfo.FileSizeInMB, LoggingConstants.CONS_TOTAL_TRACE_FILES);
                TextWriterTraceListener textWriterTraceListener = null;
                try
                {
                    traceFileStream = traceLogFilePolicy.GetLogFileStream();
                    textWriterTraceListener = new TextWriterTraceListener(traceFileStream);
                    Trace.Listeners.RemoveAt(0); //Remove the default trace listener
                    Trace.Listeners.Add(textWriterTraceListener);
                    m_IsTraceFileStreamInitialized = true;
                }
                catch (Exception ex)
                {
                    m_TraceInfo.IsTracingEnabled = false;
                    if (textWriterTraceListener != null) textWriterTraceListener.Dispose();
                    if (traceFileStream != null) traceFileStream.Dispose();
                    throw ex;
                }
            }
            catch (OMEException ObjOMEException)
            {
                LoggingHelper.ShowOMEMessage(ObjOMEException);
            }
            catch (Exception ex)
            {
                LoggingHelper.ShowMessage(ex);
            }
        }


        #region XML Documentation WriteLine
        /// <summary>
        /// Used to write a trace message into the trace file.
        /// </summary>
        /// <param name="traceMessage">Trace message to be written.</param>
        #endregion
        public static void WriteLine(string traceMessage)
        {
            if (!m_TraceInfo.IsTracingEnabled)
            {
                return;
            }

            WriteTrace(traceMessage);
        }

        public static void WriteTraceBlockStartEnd()
        {
            if (!m_TraceInfo.IsTracingEnabled)
            {
                return;
            }
           OMETrace.WriteLine(LoggingConstants.TRACEMESSAGE_LINESEPERATOR_DRAGDROP);
        }

        public static void WriteTraceInvalidCondition(string invalidCondition)
        {
            if (!m_TraceInfo.IsTracingEnabled)
            {
                return;
            }
            OMETrace.WriteLine(LoggingConstants.TRACEMESSAGE_DRAGDROP_INVALIDCONDITION +
                invalidCondition);
        }

        #region XML Documentation WriteFunctionStart
        /// <summary>
        /// Used to write function start message to the trace file.
        /// </summary>
        #endregion
        private static StringBuilder m_sbTrace = new StringBuilder(255);

        public static void WriteFunctionStart()
        {
            if (!m_TraceInfo.IsTracingEnabled)
            {
                return;
            }
            StackFrame stackFrame = new StackFrame(1);
            MethodBase methodBase = stackFrame.GetMethod();

            m_sbTrace.Append(START_FUNCTION);
            m_sbTrace.Append(methodBase.DeclaringType.Name);
            m_sbTrace.Append(LoggingConstants.DOUBLE_COLON);
            m_sbTrace.Append(methodBase.Name);

            WriteTrace(m_sbTrace.ToString());
            m_sbTrace.Remove(0, m_sbTrace.Length);
        }

        #region XML Documentation WriteFunctionEnd
        /// <summary>
        /// Used to write function end message to the trace file.
        /// </summary>
        #endregion
        public static void WriteFunctionEnd()
        {
            if (!m_TraceInfo.IsTracingEnabled)
            {
                return;
            }
            StackFrame stackFrame = new StackFrame(1);
            MethodBase methodBase = stackFrame.GetMethod();
            m_sbTrace.Append(END_FUNCTION);
            m_sbTrace.Append(methodBase.DeclaringType.Name);
            m_sbTrace.Append(LoggingConstants.DOUBLE_COLON);
            m_sbTrace.Append(methodBase.Name);

            WriteTrace(m_sbTrace.ToString());
            m_sbTrace.Remove(0, m_sbTrace.Length);

        }
        #endregion

        #region Private Static Helper Functions
        #region XML Documentation GetTraceInfoFromAppConfig
        /// <summary>
        /// This function gets the tracing information from the application configuration file
        /// </summary>
        /// <returns>TraceInfo read from the application configuration file</returns>
        #endregion
        private static TraceInfo GetTraceInfoFromAppConfig()
        {
            TraceInfo traceInfo = new TraceInfo();
            traceInfo.IsTracingEnabled = (bool)AppConfigHelper.GetValueFromConfig
                (LoggingConstants.KEY_TRACING_ENABLED, typeof(bool), traceInfo.IsTracingEnabled);
            traceInfo.FileSizeInMB =(float)AppConfigHelper.GetValueFromConfig
                (LoggingConstants.KEY_TRACE_FILE_SIZE, typeof(float), traceInfo.FileSizeInMB);
            return traceInfo;
        }

        #region XML Documentation WriteTrace
        /// <summary>
        /// This function is used to write the message to the trace file
        /// </summary>
        /// <param name="traceMessage">Trace to be returned to the trace file</param>
        #endregion
        private static void WriteTrace(string traceMessage)
        {
            try
            {
                lock (m_traceLock)
                {
                    m_sbTrace.Remove(0, m_sbTrace.Length);
                    m_sbTrace.Append(DateTime.Now.ToString());
                    m_sbTrace.Append(LoggingConstants.SPACECHARATER);
                    m_sbTrace.Append(traceMessage);
                    Trace.WriteLine(m_sbTrace.ToString());
                    Trace.Flush();
                    m_sbTrace.Remove(0, m_sbTrace.Length);
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region properties
        /// Property used to set 'enable tracing'
        /// Used when 'Enable tracing' option is selected in the 'Options' form
        static public bool IsTracingEnabled
        {
            set
            {
                try
                {
                    OMETrace.WriteFunctionStart();
                    if (value != m_TraceInfo.IsTracingEnabled)
                    {
                        m_TraceInfo.IsTracingEnabled = value;
                        InitializeTraceFileStream();
                    }
                    OMETrace.WriteFunctionEnd();
                }
                catch (OMEException ObjOMEException)
                {
                    LoggingHelper.ShowOMEMessage(ObjOMEException);
                }
                catch (Exception ex)
                {
                    LoggingHelper.ShowMessage(ex);
                }

            }
            get
            {
                return m_TraceInfo.IsTracingEnabled;
            }
        }
        #endregion properties

    }
}
