namespace OME.Logging.ExceptionLogging
{
    class LoggingConstants
    {
        #region Logging Configuration Contants
        public const float CONS_DEFAULT_EXCEPTIONLOGGING_FILE_SIZE = 2F;
        public const int CONS_TOTAL_EXCEPTIONLOGGING_FILES = 3;

        public const bool CONS_EXCEPTIONLOGGING_ENABLED = true;
        public const string CONS_EXCEPTIONLOGGING_FILE_BASE_NAME = "ExceptionLog00<<sequence>>.log";
        public const string CONS_EXCEPTIONLOGGING_LOGSINK = "File";
        public const string CONS_EXCEPTIONLOGGING_CRITICALITYLEVEL = "High";
        public const string CONS_EXCEPTIONLOGGING_FILESIZE_KEY = "FileSizeInMB";
        public const string CONS_EXCEPTIONLOGGING_LOGSINK_KEY = "LogSink";

        public const string CONS_EXCEPTIONLOGGING_KEY = "ExceptionLogging";
        public const string KEY_FILE_SIZE_IN_MB = "FileSizeInMB";
        public const string CONS_EXCEPTIONLOGGING_CRITICALITYLEVEL_KEY = "CriticalityLevel";
        public const string KEY_LOG_SINK = "LogSink";
        public const string CONS_LOG_SOURCE = "OMEException Logging"; // Event Log Source name
        public const string CONS_LOG_NAME = "OMEExceptionLog"; // Event Log Source name

        public const string CONS_TRACE_FILE_BASE_NAME = "TraceLog00<<sequence>>.log";
        public const string KEY_TRACING_ENABLED = "TracingEnabled";
        public const string KEY_TRACE_FILE_SIZE = "TraceFileSizeInMB";


        public const string KEY_DEFAULT_TRACE_FILE_SIZE = "Default_TraceFileSizeInMB";
        public const string KEY_DEFAULT_EXCEPTION_LOGGING = "Default_ExceptionLogging";
        public const string KEY_DEFAULT_LOG_SINK = "Default_LogSink";
        public const string KEY_DEFAULT_FILE_SIZE_IN_MB = "Default_FileSizeInMB";
        public const string KEY_MAXIMUM_FILE_SIZE_IN_MB = "Max_FileSizeInMB";
        public const string KEY_MINIMUM_FILE_SIZE_IN_MB = "Min_FileSizeInMB";

        

        public const string KEY_MIN_TRACE_FILE_SIZE = "MinTraceFileSizeInMB";
        
        public const int CONS_TOTAL_TRACE_FILES = 3;

        public const string EXCEPTION_SYSTEM_ERROR = "SYSTEM_ERROR_MESSAGE";

        internal const string TRACEMESSAGE_LINESEPERATOR_DRAGDROP = "***********************************";
        internal const string TRACEMESSAGE_DRAGDROP_INVALIDCONDITION = "Invalid condition: ";
        public const string DOUBLE_COLON = "::";
        internal const string SPACECHARATER = " ";
        #endregion 
    }
}
