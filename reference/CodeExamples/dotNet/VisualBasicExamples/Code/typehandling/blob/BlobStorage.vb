Imports System.IO
Imports System.Threading
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Ext
Imports Db4objects.Db4o.Types
Imports File = Sharpen.IO.File

Namespace Db4oDoc.Code.TypeHandling.Blob
    Public Class BlobStorage
        Private blob As IBlob

        Public Sub New()
        End Sub

        Public Sub ReadFileIntoDb(ByVal fileToStore As File)
            ' #example: Store the file as a db4o-blob
            blob.ReadFrom(fileToStore)
            ' #end example
            WaitTillDbIsFinished()
        End Sub

        Public Function ReadFromDbIntoFile() As File
            Dim file As File = CreateTemporaryFile()
            ' #example: Load a blob from a db4o-blob
            blob.WriteTo(file)
            ' #end example
            WaitTillDbIsFinished()
            Return file
        End Function


        ''' unfortunately there's no callback for blobs. So the only way it to poll for it
        Private Sub WaitTillDbIsFinished()
            ' #example: wait until the operation is done
            While blob.GetStatus() > Status.Completed
                Thread.Sleep(50)
            End While
            ' #end example
        End Sub

        ''' unfortunately the db4o-blob-type doesn't support streams. The only way is to use
        ''' files. Therefore we create temporary-files
        Private Shared Function CreateTemporaryFile() As File
            Dim pathOfFile As String = Path.GetTempPath() & Path.GetRandomFileName()
            Return If(Directory.Exists(pathOfFile), CreateTemporaryFile(), New File(pathOfFile))
        End Function
    End Class

    Public Class BlobExamples
        Public Shared Sub Main(ByVal args As String())
            StoreBlob()
            ReadBlob()
        End Sub

        Private Shared Sub ReadBlob()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("database.db4o")
                Dim blob As BlobStorage = container.Query(Of BlobStorage)()(0)
                Dim file As File = blob.ReadFromDbIntoFile()
            End Using
        End Sub

        Private Shared Sub StoreBlob()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("database.db4o")
                Dim blob As New BlobStorage()
                container.Store(blob)
                blob.ReadFileIntoDb(New File("C:\Pictures\IMG_1.jpg"))
            End Using
        End Sub
    End Class
End Namespace
