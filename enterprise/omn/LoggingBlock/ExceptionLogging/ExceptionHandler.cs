using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using OME.Logging.Exceptions;
using OME.Logging.Common;


namespace OME.Logging.ExceptionLogging
{
    public class ExceptionLoggingInfo
    {
        #region Private Variables
        private bool m_isLoggingEnabled = LoggingConstants.CONS_EXCEPTIONLOGGING_ENABLED;
        private string m_logSink = LoggingConstants.CONS_EXCEPTIONLOGGING_LOGSINK;
        private float m_fileSizeInMB = LoggingConstants.CONS_DEFAULT_EXCEPTIONLOGGING_FILE_SIZE;
        private string m_criticalityLevel = LoggingConstants.CONS_EXCEPTIONLOGGING_CRITICALITYLEVEL;
        #endregion

        #region Public Properties
        public bool IsLoggingEnabled
        {
            get { return m_isLoggingEnabled; }
            set { m_isLoggingEnabled = value; }
        }

        public float FileSizeInMB
        {
            get { return m_fileSizeInMB; }
            set { m_fileSizeInMB = value; }
        }

        public string LogSink
        {
            get { return m_logSink; }
            set { m_logSink = value; }
        }

        public string CriticalityLevel
        {
            get { return m_criticalityLevel; }
            set { m_criticalityLevel = value; }
        }
        #endregion
    }

    public static class ExceptionHandler 
    {
        #region Private static members
        private static ExceptionLoggingInfo m_ExceptionLoggingInfo;
        private static ILogger m_Logger;
        private static Object m_syncLock = new Object();
        private static bool m_isFileStreamInitialized = false;
        private static bool m_isEventlogInitialized = false;
        #endregion

        #region Constructor
        static ExceptionHandler()
        {
            m_ExceptionLoggingInfo = GetConfigValues();
        }
        #endregion

        #region Public Methods
        public static void Initialize()
        {
            InitialiseExceptionHandlerStreams();         
        }

        private static void InitialiseExceptionHandlerStreams()
        {
            if (false == m_ExceptionLoggingInfo.IsLoggingEnabled) return;

            if (m_ExceptionLoggingInfo.LogSink.ToUpper() == "FILE")
            {
                if (!m_isFileStreamInitialized)
                {
                    ExceptionLogFilePolicy exceptionLogFilePolicy = null;
                    FileStream logFileStream = null;
                    try
                    {
                        exceptionLogFilePolicy = new ExceptionLogFilePolicy(LoggingConstants.CONS_EXCEPTIONLOGGING_FILE_BASE_NAME,
                                                                            m_ExceptionLoggingInfo.FileSizeInMB,
                                                                            LoggingConstants.CONS_TOTAL_EXCEPTIONLOGGING_FILES);

                        logFileStream = exceptionLogFilePolicy.GetLogFileStream();
                        m_Logger = new FlatFileLogger(logFileStream);
                        m_isFileStreamInitialized = true;
                    }
                    catch (Exception ex)
                    {
                        m_ExceptionLoggingInfo.IsLoggingEnabled = false;
                        if (logFileStream != null) logFileStream.Dispose();
                        throw ex;
                    }
                }
            }
            else if (m_ExceptionLoggingInfo.LogSink.ToUpper() == "EVENTLOG")
            {
                if (!m_isEventlogInitialized)
                {
                    try
                    {
                        m_Logger = new FormattedEventLogger();
                        m_isEventlogInitialized = true;
                    }
                    catch (Exception ex)
                    {
                        m_ExceptionLoggingInfo.IsLoggingEnabled = false;
                        throw ex;
                    }
                }
                
            }
            else
            {
                //TODO: Throw some sensible error
                throw new OMEException("999", "Logging Exception", CriticalityLevel.HIGH);
            }
        }
      
        public static string HandleException(OMEException objEx)
        {
            string messageFromResourceFile =objEx.ErrorCode;
            if (!m_ExceptionLoggingInfo.IsLoggingEnabled)
            {
                return messageFromResourceFile; 
            }

            if( m_ExceptionLoggingInfo.CriticalityLevel.ToUpper()=="HIGH" )
            {
                if(objEx.criticalityLevel == CriticalityLevel.HIGH )
                {
                  LogException(objEx);
                }
            }
            else if( m_ExceptionLoggingInfo.CriticalityLevel.ToUpper()=="MEDIUM" )
            {
                if (objEx.criticalityLevel == CriticalityLevel.HIGH || objEx.criticalityLevel == CriticalityLevel.MEDIUM) 
                {
                   LogException(objEx);
                }
            }
            else if ( m_ExceptionLoggingInfo.CriticalityLevel.ToUpper() == "LOW" )
            {
                if (objEx.criticalityLevel == CriticalityLevel.HIGH || objEx.criticalityLevel == CriticalityLevel.MEDIUM || objEx.criticalityLevel == CriticalityLevel.MEDIUM || objEx.criticalityLevel == CriticalityLevel.LOW ) 
                {
                   LogException(objEx);
                }
            }
            return messageFromResourceFile; 
        }

        public static string HandleException(Exception objEx)
        {
            if (m_ExceptionLoggingInfo.IsLoggingEnabled)
            {
                LogException(objEx);
            }

            //TODO: Decide on the error code and the message
            return LoggingConstants.EXCEPTION_SYSTEM_ERROR;
        }

        public static string HandleException(OMEException objEx, int stackLevel)
        {
            string messageFromResourceFile = objEx.ErrorCode;
            if (String.IsNullOrEmpty(messageFromResourceFile))
                messageFromResourceFile = objEx.ExMessage;

            if (!m_ExceptionLoggingInfo.IsLoggingEnabled)
            {
                return messageFromResourceFile;
            }

            if (m_ExceptionLoggingInfo.CriticalityLevel.ToUpper() == "HIGH")
            {
                if (objEx.criticalityLevel == CriticalityLevel.HIGH)
                {
                    LogException(objEx, stackLevel);
                }
            }
            else if (m_ExceptionLoggingInfo.CriticalityLevel.ToUpper() == "MEDIUM")
            {
                if (objEx.criticalityLevel == CriticalityLevel.HIGH || objEx.criticalityLevel == CriticalityLevel.MEDIUM)
                {
                    LogException(objEx, stackLevel);
                }
            }
            else if (m_ExceptionLoggingInfo.CriticalityLevel.ToUpper() == "LOW")
            {
                if (objEx.criticalityLevel == CriticalityLevel.HIGH || objEx.criticalityLevel == CriticalityLevel.MEDIUM || objEx.criticalityLevel == CriticalityLevel.MEDIUM || objEx.criticalityLevel == CriticalityLevel.LOW)
                {
                    LogException(objEx, stackLevel);
                }
            }
            return messageFromResourceFile;
        }

        public static string HandleDBException(OMEException objEx, int stackLevel)
        {
            string messageFromResourceFile = objEx.ErrorCode;
            if (String.IsNullOrEmpty(messageFromResourceFile))
                messageFromResourceFile = objEx.ExMessage;

            if (!m_ExceptionLoggingInfo.IsLoggingEnabled)
            {
                return messageFromResourceFile;
            }

            if (m_ExceptionLoggingInfo.CriticalityLevel.ToUpper() == "HIGH")
            {
                if (objEx.criticalityLevel == CriticalityLevel.HIGH)
                {
                    LogDBException(objEx, stackLevel);
                }
            }
            else if (m_ExceptionLoggingInfo.CriticalityLevel.ToUpper() == "MEDIUM")
            {
                if (objEx.criticalityLevel == CriticalityLevel.HIGH || objEx.criticalityLevel == CriticalityLevel.MEDIUM)
                {
                    LogDBException(objEx, stackLevel);
                }
            }
            else if (m_ExceptionLoggingInfo.CriticalityLevel.ToUpper() == "LOW")
            {
                if (objEx.criticalityLevel == CriticalityLevel.HIGH || objEx.criticalityLevel == CriticalityLevel.MEDIUM || objEx.criticalityLevel == CriticalityLevel.MEDIUM || objEx.criticalityLevel == CriticalityLevel.LOW)
                {
                    LogDBException(objEx, stackLevel);
                }
            }
            return messageFromResourceFile;
        }


        public static string HandleException(Exception objEx, int stackLevel)
        {
            if ( m_ExceptionLoggingInfo.IsLoggingEnabled )
            {
                LogException(objEx, stackLevel);
            }

            //TODO: Decide on the error code and the message
            return LoggingConstants.EXCEPTION_SYSTEM_ERROR;
        }

        public static void UpdateExceptionLogginInfo()
        {
            m_ExceptionLoggingInfo = GetConfigValues();
            InitialiseExceptionHandlerStreams();
        }
        #endregion

        #region Private Static Helper Functions
        private static ExceptionLoggingInfo GetConfigValues()
        {
            ExceptionLoggingInfo exceptionLoggingInfo = new ExceptionLoggingInfo();
            exceptionLoggingInfo.IsLoggingEnabled = (bool)AppConfigHelper.GetValueFromConfig(LoggingConstants.CONS_EXCEPTIONLOGGING_KEY, typeof(bool), exceptionLoggingInfo.IsLoggingEnabled);
            exceptionLoggingInfo.CriticalityLevel = (string)AppConfigHelper.GetValueFromConfig(LoggingConstants.CONS_EXCEPTIONLOGGING_CRITICALITYLEVEL_KEY, typeof(string), exceptionLoggingInfo.CriticalityLevel);
            exceptionLoggingInfo.FileSizeInMB = (float)AppConfigHelper.GetValueFromConfig(LoggingConstants.CONS_EXCEPTIONLOGGING_FILESIZE_KEY, typeof(float), exceptionLoggingInfo.FileSizeInMB);
            exceptionLoggingInfo.LogSink = (string)AppConfigHelper.GetValueFromConfig(LoggingConstants.CONS_EXCEPTIONLOGGING_LOGSINK_KEY, typeof(string), exceptionLoggingInfo.LogSink);
            return exceptionLoggingInfo;
        }

       
        private static void LogException(OMEException oEx, int iLevel)
        {
            string traceInfo = null;

            if ( oEx.traceInfo == StackTraceType.ENHANCED )
            {
                 //StackTrace st = new StackTrace(true);
                 //traceInfo = OMEStackTrace.EnhancedStackTrace(st);
                 traceInfo = OMEStackTrace.EnhancedStackTrace(oEx.StackTrace);
            }
            else if (oEx.traceInfo == StackTraceType.BRIEF)
            {
                StackTrace st = new StackTrace(true);
                traceInfo = OMEStackTrace.BriefStackTrace(st, iLevel); //ilaevel is stack Frame 
            }

            string traceMessage;
            if(String.IsNullOrEmpty(oEx.ErrorCode))
                traceMessage = oEx.ExMessage;
            else
                traceMessage = oEx.ErrorCode;

            traceMessage += Environment.NewLine + oEx.AdditionalInfoToWrite;
            if ( oEx.innerException != null )
            {
                traceMessage += Environment.NewLine + oEx.innerException;
            }

            traceMessage += Environment.NewLine + traceInfo;

            lock (m_syncLock)
            {
                m_Logger.Write(traceMessage);
            }
        }
       
        
        private static void LogDBException(OMEException oEx, int iLevel)
        {
            string traceInfo = null;

            if (oEx.traceInfo == StackTraceType.ENHANCED)
            {
                //StackTrace st = new StackTrace(true);
                //traceInfo = OMEStackTrace.EnhancedStackTrace(st);
                traceInfo = OMEStackTrace.EnhancedStackTrace(oEx.StackTrace);
            }
            else if (oEx.traceInfo == StackTraceType.BRIEF)
            {
                StackTrace st = new StackTrace(true);
                traceInfo = OMEStackTrace.BriefStackTrace(st, iLevel); //ilaevel is stack Frame 
            }

            string traceMessage = oEx.SysErrorMessage;
            if (oEx.innerException != null)
            {
                traceMessage += Environment.NewLine + oEx.innerException;
            }

            traceMessage += Environment.NewLine + traceInfo;

            lock (m_syncLock)
            {
                m_Logger.Write(traceMessage);
            }

        }
        private static void LogException(Exception oEx, int iLevel)
        {

            string traceInfo = null;

            //StackTrace st = new StackTrace(true);
            //traceInfo = OMEStackTrace.EnhancedStackTrace(st);
            traceInfo = OMEStackTrace.EnhancedStackTrace(oEx.StackTrace);

            string traceMessage = LoggingConstants.EXCEPTION_SYSTEM_ERROR + Environment.NewLine + oEx.Message;
            if (oEx.InnerException != null)
            {
                traceMessage += Environment.NewLine + oEx.InnerException.Message;
            }
            traceMessage += Environment.NewLine + traceInfo;

            lock ( m_syncLock )
            {
               m_Logger.Write(traceMessage);
            }
        }

        private static void LogException(OMEException oEx)
        {
            string traceInfo = null;

            if (oEx.traceInfo == StackTraceType.ENHANCED)
            {
                //StackTrace st = new StackTrace(true);
                //traceInfo = OMEStackTrace.EnhancedStackTrace(st);
                traceInfo = OMEStackTrace.EnhancedStackTrace(oEx.StackTrace);
            }
            else if (oEx.traceInfo == StackTraceType.BRIEF)
            {
                StackTrace st = new StackTrace(true);
                traceInfo = OMEStackTrace.BriefStackTrace(st, 0); //ilaevel is stack Frame 
            }

            string traceMessage = oEx.ErrorCode;
            if (oEx.innerException != null)
            {
                traceMessage += Environment.NewLine + oEx.innerException;
            }

            traceMessage += Environment.NewLine + traceInfo;

            lock (m_syncLock)
            {
                m_Logger.Write(traceMessage);
            }
        }

        private static void LogException(Exception oEx)
        {

            string traceInfo = null;
            //string systemErrorCode = "SYS001";
            //StackTrace st = new StackTrace(true);
            //traceInfo = OMEStackTrace.EnhancedStackTrace(st);
            traceInfo = OMEStackTrace.EnhancedStackTrace(oEx.StackTrace);
            string traceMessage = LoggingConstants.EXCEPTION_SYSTEM_ERROR + Environment.NewLine + oEx.Message;
            if (oEx.InnerException != null)
            {
                traceMessage += Environment.NewLine + oEx.InnerException.Message;
            }
            traceMessage += Environment.NewLine + traceInfo;

            lock (m_syncLock)
            {
                m_Logger.Write(traceMessage);
            }
        }
      
        #endregion
    }
}
