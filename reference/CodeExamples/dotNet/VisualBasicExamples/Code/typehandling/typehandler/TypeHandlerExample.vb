Imports System.Text
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Reflect
Imports Db4objects.Db4o.Typehandlers

Namespace Db4oDoc.Code.TypeHandling.TypeHandler
    Public Class TypeHandlerExample
        Public Shared Sub Main(ByVal args As String())
            Using container As IObjectContainer = OpenContainer()
                ' #example: Store the non storable type
                Dim testType As New MyType()
                ' #end example
                container.Store(testType)
            End Using

            Using container As IObjectContainer = OpenContainer()
                ' #example: Load the non storable type
                Dim builder As MyType = container.Query(Of MyType)()(0)
                ' #end example
                Console.WriteLine(builder)
            End Using
        End Sub

        Private Shared Function OpenContainer() As IObjectContainer
            ' #example: Register type handler
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.RegisterTypeHandler(
                New SingleClassTypeHandlerPredicate(GetType(StringBuilder)), New StringBuilderHandler())
            ' #end example
            Return Db4oEmbedded.OpenFile(configuration, "database.db4o")
        End Function

        Private Class MyType
            Private builder As New StringBuilder("TestData")

            Public Overrides Function ToString() As String
                Return String.Format("Builder: {0}", builder)
            End Function
        End Class
    End Class
End Namespace
