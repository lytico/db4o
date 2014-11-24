using System;
using System.IO;

namespace Db4oTutorial.ExampleRunner
{
    /// <summary>
    /// Why do we need this? We cannot use Console.SetOut() on silverlight.
    /// The hack is that we replace it with this class
    /// 
    /// </summary>
    public class ConsoleOutReplacement
    {
        [ThreadStatic] private static StringWriter contextWriter;

        public static string RunInContext(Action action)
        {
            try
            {
                contextWriter = new StringWriter();
                action();
                return GetText();
            }
            finally
            {
                contextWriter = null;
            }
        }

        public static TextWriter Out
        {
            get
            {
                CheckState();
                return contextWriter;
            }
        }

        private static void CheckState()
        {
            if (null == contextWriter)
            {
                throw new InvalidOperationException(
                    "Cannot access writer outside a context. Run with ConsoleOutReplacement.RunInContext");
            }
        }

        public static string GetText()
        {
            CheckState();
            return contextWriter.ToString();
        }



    }
}