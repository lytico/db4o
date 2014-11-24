using OME.Logging.Common; 

namespace OME.Logging.Tracing
{
    class TraceLogFilePolicy : LogFilePolicy
    {
        #region Public Constructor
        public TraceLogFilePolicy(string logFileBaseName, float maxLogFileSize, int rotationFileCount):base(logFileBaseName, maxLogFileSize, rotationFileCount)
        {
        }
        #endregion
    }
}
