Imports System.Diagnostics

Namespace Db4oDoc.Code.Performance
    Public NotInheritable Class StopWatchUtil
        Private Sub New()
        End Sub
        Public Shared Sub Time(taskToTime As Action)
            Dim st = Stopwatch.StartNew()
            taskToTime()
            Console.Out.WriteLine("Time elapsed: {0}", st.ElapsedMilliseconds)
        End Sub
    End Class
End Namespace
