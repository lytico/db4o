Imports Db4objects.Db4o.IO
Imports Sharpen.Lang

Namespace Db4oDoc.Code.Configuration.IO
    ' #example: A logging bin decorator
    Public Class LoggingBin
        Inherits BinDecorator
        Implements IBin
        Public Sub New(bin As IBin)
            MyBase.New(bin)
        End Sub


        Public Overrides Sub Close()
            Console.WriteLine("Called LoggingBin.Close()")
            MyBase.Close()
        End Sub


        Public Overrides Function Length() As Long
            Console.WriteLine("Called LoggingBin.Length()")
            Return MyBase.Length()
        End Function


        Public Overrides Function Read(position As Long, bytes As Byte(), bytesToRead As Integer) As Integer
            Console.WriteLine("Called LoggingBin.Read(" & position & ", ...," & bytesToRead & ")")
            Return MyBase.Read(position, bytes, bytesToRead)
        End Function


        Public Overrides Sub Sync()
            Console.WriteLine("Called LoggingBin.Sync()")
            MyBase.Sync()
        End Sub


        Public Overrides Function SyncRead(position As Long, bytes As Byte(), bytesToRead As Integer) As Integer
            Console.WriteLine("Called LoggingBin.SyncRead(" & position & ", ...," & bytesToRead & ")")
            Return MyBase.SyncRead(position, bytes, bytesToRead)
        End Function


        Public Overrides Sub Write(position As Long, bytes As Byte(), bytesToWrite As Integer)
            Console.WriteLine("Called LoggingBin.Write(" & position & ", ...," & bytesToWrite & ")")
            MyBase.Write(position, bytes, bytesToWrite)
        End Sub


        Public Overrides Sub Sync(runnable As IRunnable)
            Console.WriteLine("Called LoggingBin.Sync(" & Convert.ToString(runnable) & ")")
            MyBase.Sync(runnable)
        End Sub
    End Class
    ' #end example
End Namespace
