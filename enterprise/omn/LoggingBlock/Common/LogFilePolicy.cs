using System;
using System.IO;
using System.Windows.Forms;

namespace OME.Logging.Common
{
    class LogFilePolicy
    {
        #region Protected Member Variables
        protected string m_logFileBaseName;
        protected float m_maxLogFileSizeInMB;
        protected int m_rotationFileCount;
        #endregion

        #region Constructors
        public LogFilePolicy(string logFileBaseName, float maxLogFileSize, int rotationFileCount)
        {
            m_logFileBaseName = logFileBaseName;
            m_maxLogFileSizeInMB = maxLogFileSize;
            m_rotationFileCount = rotationFileCount;
        }
        #endregion

        #region Public Functions
        public virtual FileStream GetLogFileStream()
        {
#if DEBUG

            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "db4objects" + Path.DirectorySeparatorChar + "ObjectManagerEnterprise"))
            {

                //Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "db4objects");
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "db4objects" + Path.DirectorySeparatorChar + "ObjectManagerEnterprise");

            }
#endif

            //string appDir = Path.GetDirectoryName(Application.ExecutablePath);
            string appDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                Path.DirectorySeparatorChar + "db4objects" + Path.DirectorySeparatorChar + "ObjectManagerEnterprise";
            //string traceFileNameWithPath = appDir + Path.DirectorySeparatorChar + m_logFileBaseName.Replace("<<sequence>>", "1");
            string traceFileNameWithPath = appDir + Path.DirectorySeparatorChar + m_logFileBaseName.Replace("<<sequence>>", "1");
            FileMode fileOpenMode = FileMode.Truncate;

            FileInfo traceFileInfo = null;
            string traceFileNameTemp = null;
            float traceFileSizeInMB = 0F;
            for (int iTraceIndex = 1; iTraceIndex <= m_rotationFileCount; iTraceIndex++)
            {
                traceFileNameTemp = appDir + Path.DirectorySeparatorChar + m_logFileBaseName.Replace("<<sequence>>", iTraceIndex.ToString());
                traceFileInfo = new FileInfo(traceFileNameTemp);

                if ( !traceFileInfo.Exists )
                {
                    traceFileNameWithPath = traceFileNameTemp;
                    fileOpenMode = FileMode.CreateNew;
                    break;
                }

                traceFileSizeInMB = (float)((traceFileInfo.Length / 1000F) / 1000F);
                if ( traceFileSizeInMB >= m_maxLogFileSizeInMB )
                {
                    continue;
                }
                else
                {
                    traceFileNameWithPath = traceFileNameTemp;
                    fileOpenMode = FileMode.Append;
                    break;
                }
            }

            FileStream traceFileStream = new FileStream(traceFileNameWithPath, fileOpenMode, FileAccess.Write,FileShare.ReadWrite  );
            return traceFileStream;
        }
        #endregion
    }
}
