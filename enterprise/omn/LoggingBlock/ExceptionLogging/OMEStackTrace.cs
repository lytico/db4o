using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace OME.Logging.ExceptionLogging
{
    public class OMEStackTrace
    {
        #region Public Static Members 
        public static Assembly m_parentAssembly;
        #endregion
        
        #region Public Static Methods
        public static string EnhancedStackTrace(StackTrace stackTrace)
        {
            Int32 iFrame;
            StringBuilder sbTraceInfo = new StringBuilder();
            sbTraceInfo.Append(Environment.NewLine);
            sbTraceInfo.Append("--- Stack Trace ---");
            sbTraceInfo.Append(Environment.NewLine);
            sbTraceInfo.Append("Time Stamp : ");
            sbTraceInfo.Append(DateTime.Now.ToLongTimeString().ToString());
            sbTraceInfo.Append(Environment.NewLine);

            for (iFrame = 0; iFrame < stackTrace.FrameCount - 1; iFrame++)
            {
                StackFrame sf = stackTrace.GetFrame(iFrame);
                sbTraceInfo.Append(StackFrameToString(sf));
            }
            return sbTraceInfo.ToString();
        }

        public static string EnhancedStackTrace(string stackTrace)
        {
            //Int32 iFrame;
            StringBuilder sbTraceInfo = new StringBuilder();
            sbTraceInfo.Append(Environment.NewLine);
            sbTraceInfo.Append("--- Stack Trace ---");
            sbTraceInfo.Append(Environment.NewLine);
            sbTraceInfo.Append("Time Stamp : ");
            sbTraceInfo.Append(DateTime.Now.ToLongTimeString().ToString());
            sbTraceInfo.Append(Environment.NewLine);
            sbTraceInfo.Append(stackTrace);
            return sbTraceInfo.ToString();
        }

        public static string BriefStackTrace(StackTrace stackTrace, int ilevel)
        {   
            StringBuilder sbTraceInfo = new StringBuilder();
            sbTraceInfo.Append(Environment.NewLine);
            sbTraceInfo.Append("--- Stack Trace ---");
            sbTraceInfo.Append(Environment.NewLine);
            sbTraceInfo.Append("Time Stamp : ");
            sbTraceInfo.Append(DateTime.Now.ToLongTimeString().ToString());
            sbTraceInfo.Append(Environment.NewLine);

            StackFrame sf = stackTrace.GetFrame(2 + ilevel);
            sbTraceInfo.Append(StackFrameToString(sf));
            
            return sbTraceInfo.ToString();
        }
        public static string BriefStackTrace(int ilevel)
        {
            StringBuilder sbTraceInfo = new StringBuilder();
            sbTraceInfo.Append(Environment.NewLine);
            sbTraceInfo.Append("--- Stack Trace ---");
            sbTraceInfo.Append(Environment.NewLine);
            sbTraceInfo.Append("Time Stamp :    " + DateTime.Now.ToLongTimeString());
            sbTraceInfo.Append(Environment.NewLine);
            sbTraceInfo.Append("Class Name :    " + new StackTrace().GetFrame(ilevel).GetMethod().ReflectedType.FullName);
            sbTraceInfo.Append(Environment.NewLine);
            sbTraceInfo.Append("Method Name :   " + new StackTrace().GetFrame(ilevel).GetMethod().ToString());
            sbTraceInfo.Append(Environment.NewLine);
            sbTraceInfo.Append("Line Number :   ");
            sbTraceInfo.Append(Environment.NewLine);

            return sbTraceInfo.ToString();
        }
        public static string EnhancedStackTrace()
        {
            Int32 iFrame;
            Int32 frameCount = new StackTrace().FrameCount;
            StringBuilder sbTraceInfo = new StringBuilder();
            
            sbTraceInfo.Append(Environment.NewLine);
            sbTraceInfo.Append("--- Stack Trace ---");
            sbTraceInfo.Append(Environment.NewLine);

            for (iFrame = 0; iFrame < frameCount - 1; iFrame++)
            {
                sbTraceInfo.Append("Time Stamp :    " + DateTime.Now.ToLongTimeString());
                sbTraceInfo.Append(Environment.NewLine);
                sbTraceInfo.Append("Class Name :    " + new StackTrace().GetFrame(iFrame).GetMethod().ReflectedType.FullName);
                sbTraceInfo.Append(Environment.NewLine);
                sbTraceInfo.Append("Method Name :   " + new StackTrace().GetFrame(iFrame).GetMethod().ToString());
                sbTraceInfo.Append(Environment.NewLine);
            }

            return sbTraceInfo.ToString();
        }
        #endregion 

        #region Private Static Helper Methods
        private static Assembly ParentAssembly()
        {
            if ( m_parentAssembly == null )
            {
                if ( Assembly.GetEntryAssembly() == null )
                    m_parentAssembly = System.Reflection.Assembly.GetCallingAssembly();
                else
                    m_parentAssembly = System.Reflection.Assembly.GetEntryAssembly();
            }
            return m_parentAssembly;
        }
        private static string StackFrameToString(StackFrame sf)
        {
            StringBuilder sb = new StringBuilder();
            Int32 intParam;
            MemberInfo mi = sf.GetMethod();

            //'-- build method name
            sb.Append("   ");
            sb.Append(mi.DeclaringType.Namespace);
            sb.Append(".");
            sb.Append(mi.DeclaringType.Name);
            sb.Append(".");
            sb.Append(mi.Name);

            //'-- build method params
            ParameterInfo[] objParameters = sf.GetMethod().GetParameters();
            sb.Append("(");
            intParam = 0;
            foreach (ParameterInfo objParameter in objParameters)
            {
                intParam += 1;
                if (intParam > 1)
                    sb.Append(", ");
                sb.Append(objParameter.Name);
                sb.Append(" As ");
                sb.Append(objParameter.ParameterType.Name);
            }
            sb.Append(")");
            sb.Append(Environment.NewLine);

            //'-- if source code is available, append location info
            sb.Append("       ");
            if ((sf.GetFileName() == null) || (sf.GetFileName().Length == 0))
            {
                sb.Append(System.IO.Path.GetFileName(ParentAssembly().CodeBase));
                //'-- native code offset is always available
                sb.Append(": N ");
                sb.Append(String.Format("{0:#00000}", sf.GetNativeOffset()));
            }
            else
            {
                sb.Append(System.IO.Path.GetFileName(sf.GetFileName()));
                sb.Append(": line ");
                sb.Append(String.Format("{0:#0000}", sf.GetFileLineNumber()));
                sb.Append(", col ");
                sb.Append(String.Format("{0:#00}", sf.GetFileColumnNumber()));
                //'-- if IL is available, append IL location info
                if (sf.GetILOffset() != StackFrame.OFFSET_UNKNOWN)
                {
                    sb.Append(", IL ");
                    sb.Append(String.Format("{0:#0000}", sf.GetILOffset()));
                }
            }
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }
        #endregion 
    }
}
