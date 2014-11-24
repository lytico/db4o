Imports Db4objects.Db4o.IO

Namespace Db4oDoc.Code.Configuration.IO
    ' #example: A logging storage decorator
    Public Class LoggingStorage
        Inherits StorageDecorator
        Implements IStorage
        Public Sub New(storage As IStorage)
            MyBase.New(storage)
        End Sub

        Public Overrides Function Exists(uri As String) As Boolean
            Console.WriteLine("Called: LoggingStorage.Exists(" & uri & ")")
            Return MyBase.Exists(uri)
        End Function

        Public Overrides Function Open(config As BinConfiguration) As IBin
            Console.WriteLine("Called: LoggingStorage.Open(" & Convert.ToString(config) & ")")
            Return MyBase.Open(config)
        End Function

        Protected Overrides Function Decorate(config As BinConfiguration, bin As IBin) As IBin
            Return New LoggingBin(MyBase.Decorate(config, bin))
        End Function

        Public Overrides Sub Delete(uri As String)
            Console.WriteLine("Called: LoggingStorage.Delete(" & uri & ")")
            MyBase.Delete(uri)
        End Sub

        Public Overrides Sub Rename(oldUri As String, newUri As String)
            Console.WriteLine("Called: LoggingStorage.Rename(" & oldUri & "," & newUri & ")")
            MyBase.Rename(oldUri, newUri)
        End Sub
    End Class
    '#end example
End Namespace
