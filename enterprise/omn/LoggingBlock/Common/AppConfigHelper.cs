using System;
using System.Configuration;
using OME.Logging.Exceptions;
using OME.Logging.ExceptionLogging;
using OME.Logging.Tracing;
using System.Windows.Forms;

namespace OME.Logging.Common
{
    public class AppConfigHelper
    {
        #region Member variables
        private static int m_InvalidIntValue = -1;
        private static int m_SessionTimeOutInterval = AppConfigHelper.m_InvalidIntValue;
        private static int m_LdapConnectionTimeout = AppConfigHelper.m_InvalidIntValue;

        private static string m_TracingEnabled;
        private static float m_TraceFileSizeInMB = -1;
        private static string m_ExceptionLogging;
        private static string m_LogSink;
        private static float m_FileSizeInMB = -1;
        private static string m_CriticalityLevel;

        private static int m_Default_TraceFileSizeInMB = -1;
        private static int m_Minimum_TraceFileSizeInMB = -1;
        private static string m_Default_ExceptionLogging;
        private static string m_Default_LogSink;
        private static int m_Default_FileSizeInMB = -1;
        private static int m_Maximum_FileSizeInMB = -1;
        private static int m_Minimum_FileSizeInMB = -1;

        // Used For Virtualization of users in case of IDU Joins
        private static string m_Idu_Nary_Join = string.Empty;
        private static string m_Idu_Aggregate_Join = string.Empty;

        const string KEY_MISSING_MESSAGE = "APPLICATION_CONFIG_KEY_MISSING_MESSAGE";

        #endregion

        #region Constructor
        private AppConfigHelper() { }
        #endregion Constructor

        #region Properties
        
        /// <summary>
        /// Get or set invalid Integer value (e.i. -1)
        /// </summary>
        public static int InvalidIntegerValue
        {
            get { return AppConfigHelper.m_InvalidIntValue; }
        }

      
        
        public static bool TracingEnabled
        {
            get 
            {
                if (AppConfigHelper.m_TracingEnabled == string.Empty)
                {
                    AppConfigHelper.m_TracingEnabled =
                        (string)AppConfigHelper.GetValueFromConfig(
                        LoggingConstants.KEY_TRACING_ENABLED, typeof(string), "true");
                } 
                return Convert.ToBoolean(AppConfigHelper.m_TracingEnabled) ; 
            }
            set { AppConfigHelper.m_TracingEnabled = value.ToString(); }
        }

        public static float TraceFileSizeInMB
        {
            get 
            {
                if (AppConfigHelper.m_TraceFileSizeInMB == m_InvalidIntValue)
                {
                    AppConfigHelper.m_TraceFileSizeInMB =
                        (float)AppConfigHelper.GetValueFromConfig(
                        LoggingConstants.KEY_TRACE_FILE_SIZE, typeof(float), 2.0F);
                }
                return AppConfigHelper.m_TraceFileSizeInMB; 
            }
            set { AppConfigHelper.m_TraceFileSizeInMB = value; }
        }
        
        public static bool ExceptionLogging
        {
            get 
            {
                if (AppConfigHelper.m_ExceptionLogging == string.Empty)
                {
                    AppConfigHelper.m_ExceptionLogging =
                        (string)AppConfigHelper.GetValueFromConfig(
                        LoggingConstants.CONS_EXCEPTIONLOGGING_KEY, typeof(string), LoggingConstants.CONS_EXCEPTIONLOGGING_ENABLED);
                }
                
               return  Convert.ToBoolean(AppConfigHelper.m_ExceptionLogging);
            }
            set { AppConfigHelper.m_ExceptionLogging = value.ToString(); }
        }

        public static string LogSink
        {
            get 
            {
                if (AppConfigHelper.m_LogSink == string.Empty)
                {
                    AppConfigHelper.m_LogSink =
                        (string)AppConfigHelper.GetValueFromConfig(
                        LoggingConstants.KEY_LOG_SINK, typeof(string), LoggingConstants.CONS_EXCEPTIONLOGGING_LOGSINK);
                }
                
               return  AppConfigHelper.m_LogSink; 
            }
            set { AppConfigHelper.m_LogSink = value; }
        }

        public static float FileSizeInMB
        {
            get 
            {
                if (AppConfigHelper.m_FileSizeInMB == m_InvalidIntValue)
                {
                    AppConfigHelper.m_FileSizeInMB =
                        (float)AppConfigHelper.GetValueFromConfig(
                        LoggingConstants.KEY_FILE_SIZE_IN_MB,
                        typeof(float), LoggingConstants.CONS_DEFAULT_EXCEPTIONLOGGING_FILE_SIZE);
                }
                
                return AppConfigHelper.m_FileSizeInMB;
            }
            
            set { AppConfigHelper.m_FileSizeInMB = value; }
        }

        public static string CriticalityLevel
        {
            get 
            {
                if (AppConfigHelper.m_CriticalityLevel == string.Empty)
                {
                    AppConfigHelper.m_CriticalityLevel =
                        (string)AppConfigHelper.GetValueFromConfig(
                        LoggingConstants.CONS_EXCEPTIONLOGGING_CRITICALITYLEVEL_KEY, typeof(string), LoggingConstants.CONS_EXCEPTIONLOGGING_CRITICALITYLEVEL);
                }
                return AppConfigHelper.m_CriticalityLevel;
            }

            set { AppConfigHelper.m_CriticalityLevel = value; }
        }
      

        //public static string LanguagePreference
        //{
        //    get 
        //    {
        //        if (AppConfigHelper.m_LanguagePreference == string.Empty)
        //        {
        //            AppConfigHelper.m_LanguagePreference =
        //                    (string)AppConfigHelper.GetValueFromConfig(
        //                    Preferences.KEY_TIME_INTERVAL, typeof(string), "English"); 
        //        }   
        //        return AppConfigHelper.m_LanguagePreference;
        //    }
        //    set { AppConfigHelper.m_LanguagePreference = value; }
        //}

       

        public static int DefaultTraceFileSizeInMB
        {
            get
            {
                if (AppConfigHelper.m_Default_TraceFileSizeInMB == AppConfigHelper.m_InvalidIntValue)
                {
                    AppConfigHelper.m_Default_TraceFileSizeInMB =
                            (int)AppConfigHelper.GetValueFromConfig(
                            LoggingConstants.KEY_DEFAULT_TRACE_FILE_SIZE, typeof(int), 2);
                } 
                return AppConfigHelper.m_Default_TraceFileSizeInMB; 
            
            }
            set { AppConfigHelper.m_Default_TraceFileSizeInMB = value; }
        }

        public static int MinimumTraceFileSizeInMB
        {
            get
            {
                if (AppConfigHelper.m_Minimum_TraceFileSizeInMB == AppConfigHelper.m_InvalidIntValue)
                {
                    AppConfigHelper.m_Minimum_TraceFileSizeInMB =
                            (int)AppConfigHelper.GetValueFromConfig(
                            LoggingConstants.KEY_MIN_TRACE_FILE_SIZE, typeof(int), 2);
                } 
                return AppConfigHelper.m_Minimum_TraceFileSizeInMB; 
            }
            set { AppConfigHelper.m_Minimum_TraceFileSizeInMB = value; }
        }

        public static bool DefaultExceptionLogging
        {
            get
            {
                if (AppConfigHelper.m_Default_ExceptionLogging == string.Empty)
                {
                    AppConfigHelper.m_Default_ExceptionLogging =
                            (string)AppConfigHelper.GetValueFromConfig(
                            LoggingConstants.KEY_DEFAULT_EXCEPTION_LOGGING, typeof(string), "true");
                } 
                return Convert.ToBoolean(AppConfigHelper.m_Default_ExceptionLogging);
            }
            set { AppConfigHelper.m_Default_ExceptionLogging = value.ToString(); }
        }

        public static string DefaultLogSink
        {
            get
            {
                if (AppConfigHelper.m_Default_LogSink == string.Empty)
                {
                    AppConfigHelper.m_Default_LogSink =
                            (string)AppConfigHelper.GetValueFromConfig(
                            LoggingConstants.KEY_DEFAULT_LOG_SINK, typeof(string), LoggingConstants.CONS_EXCEPTIONLOGGING_LOGSINK);
                }
                return AppConfigHelper.m_Default_LogSink;
            }
            set { AppConfigHelper.m_Default_LogSink = value; }
        }

        public static int DefaultFileSizeInMB
        {
            get 
            {
                if (AppConfigHelper.m_Default_FileSizeInMB == m_InvalidIntValue)
                {
                    AppConfigHelper.m_Default_FileSizeInMB =
                            (int)AppConfigHelper.GetValueFromConfig(
                            LoggingConstants.KEY_DEFAULT_FILE_SIZE_IN_MB, typeof(int), 2);
                }
                return AppConfigHelper.m_Default_FileSizeInMB; 
            }
            set { AppConfigHelper.m_Default_FileSizeInMB = value; }
        }

        public static int MaximumFileSizeInMB
        {
            get 
            {
                if (AppConfigHelper.m_Maximum_FileSizeInMB == m_InvalidIntValue)
                {
                    AppConfigHelper.m_Maximum_FileSizeInMB =
                            (int)AppConfigHelper.GetValueFromConfig(
                            LoggingConstants.KEY_MAXIMUM_FILE_SIZE_IN_MB, typeof(int), 5);
                }
                return AppConfigHelper.m_Maximum_FileSizeInMB;
            }
            set { AppConfigHelper.m_Maximum_FileSizeInMB = value; }
        }

        public static int MinimumFileSizeInMB
        {
            get 
            {
                if (AppConfigHelper.m_Minimum_FileSizeInMB == m_InvalidIntValue)
                {
                    AppConfigHelper.m_Minimum_FileSizeInMB =
                            (int)AppConfigHelper.GetValueFromConfig(
                            LoggingConstants.KEY_MINIMUM_FILE_SIZE_IN_MB, typeof(int), 1);
                }  
                return AppConfigHelper.m_Minimum_FileSizeInMB;
            }
            set { AppConfigHelper.m_Minimum_FileSizeInMB = value; }
        }

        //public static string Default_LanguagePreference
        //{
        //    get
        //    {
        //        if (AppConfigHelper.m_Default_LanguagePreference == string.Empty)
        //        {
        //            AppConfigHelper.m_Default_LanguagePreference =
        //                    (string)AppConfigHelper.GetValueFromConfig(
        //                    Preferences.KEY_DEFAULT_LANGUAGE_PREFERENCE, typeof(string), "English");
        //        }
        //        return AppConfigHelper.m_Default_LanguagePreference;
        //    }
        //    set { AppConfigHelper.m_Default_LanguagePreference = value; }
        //}

       
        #endregion   Properties

        #region Public Static Functions

        #region XML Documentation GetValueFromConfig
        /// <summary>
        /// Given a key this function retrieves the value associated with the key
        /// </summary>
        /// <param name="key">key denotes the key in a application configuration file</param>
        /// <param name="type">type of the value to be retrieved</param>
        /// <param name="defaultValue">value to be returned if the function fails to retrieve the value from the configuration file</param>
        /// <returns>Returns value associated with the specified key</returns>
        #endregion
        public static object GetValueFromConfig(string key, Type type, object defaultValue)
        {
            string strValue = ConfigurationManager.AppSettings[key];
            object objValue = null;
            try
            {
                if(string.IsNullOrEmpty(strValue))                
                {
                    //OMETrace.WriteLine(Helper.GetResourceString(KEY_MISSING_MESSAGE) + key);
                    strValue = defaultValue.ToString();
                }
                objValue = Convert.ChangeType(strValue, type);
                if (objValue == null) objValue = defaultValue; 
            }
            catch (OMEException objAIException)
            {
                OMETrace.WriteLine(objAIException.Message);
                objValue  = defaultValue;
            }
            catch (Exception objException)
            {
                OMETrace.WriteLine(objException.Message);
                objValue  = defaultValue;
            } 
            return objValue;
        }
        #endregion
    }


    /// <summary>
    /// 
    /// </summary>
    public class LoggingHelper
    {
        public static void ShowMessage(Exception objException)
        {
            ExceptionHandler.HandleException(objException, 1);
            MessageBox.Show(objException.Message, Application.ProductName,
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        /// <summary>
        /// Show error messege
        /// </summary>
        /// <param name="objException"></param>
        public static void ShowOMEMessage(OMEException objException)
        {
            string str = string.Empty;
            string message = string.Empty;
            str = ExceptionHandler.HandleException(objException, 1);

            message = objException.Message.ToString();
            if (String.IsNullOrEmpty(objException.ErrorCode))
                message = objException.ExMessage;
            OMETrace.WriteLine(string.Empty);
            OMETrace.WriteLine(message);
            OMETrace.WriteLine(objException.StackTrace);
            MessageBox.Show(str, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Log the system exception to the specified LogSink
        /// </summary>
        /// <param name="objException"></param>
        public static void HandleException(Exception objException)
        {
            ExceptionHandler.HandleException(objException, 1);
        }

        /// <summary>
        /// Log the custom/user exception to the specified log sink
        /// </summary>
        /// <param name="objectException"></param>
        public static void HandleException(OMEException objectException)
        {
            string str = string.Empty;
            string message = string.Empty;
            str = ExceptionHandler.HandleException(objectException, 1);

            message = objectException.Message.ToString();
            if (String.IsNullOrEmpty(objectException.ErrorCode))
                message = objectException.ExMessage;
            OMETrace.WriteLine(string.Empty);
            OMETrace.WriteLine(message);
            OMETrace.WriteLine(objectException.StackTrace);
        }


    }
}
