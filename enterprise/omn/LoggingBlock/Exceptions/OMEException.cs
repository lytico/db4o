using System;

namespace OME.Logging.Exceptions
{
    public enum CriticalityLevel
    {
        LOW = 1,
        MEDIUM,
        HIGH
    }

    public enum StackTraceType
    {
        BRIEF = 1,
        ENHANCED
    }

    public class OMEException : Exception
    {
        #region Public Members
        public string ErrorCode;//ExMassage is a constant string that is defined in Constant.cs
        public string ExMessage;
        public string SysErrorMessage;
         public string AdditionalInfoToWrite; // Additional information internaly in the exception log.
        public CriticalityLevel criticalityLevel; //Defined in Enum.cs
        public StackTraceType traceInfo;
        public Exception innerException; 
        #endregion 

        #region Constructor and Destructor
        public OMEException()
        {
            ExMessage = string.Empty;
            criticalityLevel = CriticalityLevel.LOW;
            traceInfo = StackTraceType.BRIEF;
            
        }

        public OMEException(string errorCode, Exception ex,CriticalityLevel level)
        {
            ErrorCode = errorCode;
            innerException = ex ;
            criticalityLevel = level ;
            traceInfo = StackTraceType.BRIEF;
        }

        public OMEException(string errorCode, string errorMessage,CriticalityLevel level)
        {
            ErrorCode = errorCode;
            ExMessage = errorMessage; 
            
            criticalityLevel =level;
            traceInfo = StackTraceType.BRIEF;
        }

        public OMEException(string errorCode, string errorMessage, CriticalityLevel level, Exception innerException)
        {
            ErrorCode = errorCode;
            ExMessage = errorMessage;
            this.innerException = innerException;
            criticalityLevel = level;
            traceInfo = StackTraceType.BRIEF;
        }


        public OMEException(string errorCode, string errorMessage, CriticalityLevel level, string additionalInfoToWrite)
        {
            ErrorCode = errorCode;
            ExMessage = errorMessage;
            AdditionalInfoToWrite = additionalInfoToWrite;
            criticalityLevel = level;
            traceInfo = StackTraceType.BRIEF;
        }

        public OMEException(string errorCode, Exception ex, CriticalityLevel level, StackTraceType traceinfo)
        {
            ErrorCode = errorCode;             
            innerException = ex;
            criticalityLevel = level;
            traceInfo = traceinfo;
        }

        public OMEException(string errorCode, CriticalityLevel level, StackTraceType traceinfo)
        {
            ErrorCode = errorCode;
            criticalityLevel = level;
            traceInfo = traceinfo;
        }

        public OMEException(string errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ExMessage = errorMessage;
            criticalityLevel = CriticalityLevel.HIGH;
            traceInfo = StackTraceType.ENHANCED;
        }

        #endregion 
    }
}
