Imports System.Text
Imports Db4objects.Db4o.Internal
Imports Db4objects.Db4o.Internal.Delete
Imports Db4objects.Db4o.Marshall
Imports Db4objects.Db4o.Typehandlers

Namespace Db4oDoc.Code.TypeHandling.TypeHandler
    '''
    ''' This is a very simple example for a type handler.
    ''' Take a look at the existing db4o type handlers.
    Friend Class StringBuilderHandler
        Implements IValueTypeHandler


        ' #example: Delete the content
        Public Sub Delete(ByVal deleteContext As IDeleteContext) _
            Implements IValueTypeHandler.Delete
            SkipData(deleteContext)
        End Sub

        Private Shared Sub SkipData(ByVal deleteContext As IReadBuffer)
            Dim numBytes As Integer = deleteContext.ReadInt()
            deleteContext.Seek(deleteContext.Offset() + numBytes)
        End Sub
        ' #end example

        ' #example: Defragment the content
        Public Sub Defragment(ByVal defragmentContext As IDefragmentContext) _
            Implements IValueTypeHandler.Defragment
            SkipData(defragmentContext)
        End Sub
        ' #end example

        ' #example: Write the StringBuilder
        Public Sub Write(ByVal writeContext As IWriteContext, ByVal o As Object) _
            Implements IValueTypeHandler.Write
            Dim builder As StringBuilder = DirectCast(o, StringBuilder)
            Dim str As String = builder.ToString()
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(str)
            writeContext.WriteInt(bytes.Length)
            writeContext.WriteBytes(bytes)
        End Sub
        ' #end example

        ' #example: Read the StringBuilder
        Public Function Read(ByVal readContext As IReadContext) As Object _
            Implements IValueTypeHandler.Read
            Dim length As Integer = readContext.ReadInt()
            Dim data As Byte() = New Byte(length - 1) {}
            readContext.ReadBytes(data)
            Return New StringBuilder(Encoding.UTF8.GetString(data))
        End Function
        ' #end example
    End Class
End Namespace
