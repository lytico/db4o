using System;

using OME.Logging.Common;

namespace OME.Logging.ExceptionLogging
{
    class ExceptionLogFilePolicy : LogFilePolicy
    {
        #region Public Constructor
        public ExceptionLogFilePolicy(string logFileBaseName, float maxLogFileSize, int rotationFileCount): base(logFileBaseName, maxLogFileSize, rotationFileCount)
        {
        }
        #endregion
    }
}
