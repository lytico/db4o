Imports Db4objects.Db4o.Monitoring

Namespace Db4oDoc.Code.Tuning.Monitoring
    Public Class InstallPerformanceCounters
        Public Shared Sub Main(ByVal args As String())
            ' #example: Install the performance counters
            Db4oPerformanceCounters.Install()
            ' #end example
        End Sub
    End Class
End Namespace