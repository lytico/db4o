Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config

Namespace Db4oDoc.Code.TypeHandling.Translator
    Public Class TranslatorExample
        Public Shared Sub Main(ByVal args As String())
            Using container As IObjectContainer = CreateDB()
                ' #example: Store the non storable type
                container.Store(New NonStorableType("TestData"))
                ' #end example
            End Using
            Using container As IObjectContainer = CreateDB()
                ' #example: Load the non storable type
                Dim data As NonStorableType = container.Query(Of NonStorableType)()(0)
                ' #end example
                Console.Out.WriteLine(data.Data)
            End Using
        End Sub

        Private Shared Function CreateDB() As IObjectContainer
            ' #example: Register type translator for the NonStorableType-class
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(NonStorableType)).Translate(New ExampleTranslator())
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            ' #end example
            Return container
        End Function
    End Class


    ' #example: An example translator
    Friend Class ExampleTranslator
        Implements IObjectConstructor
        ' This is called to store the object
        Public Function OnStore(ByVal objectContainer As IObjectContainer, _
                                ByVal objToStore As Object) As Object _
                            Implements IObjectConstructor.OnStore

            Dim notStorable As NonStorableType = DirectCast(objToStore, NonStorableType)
            Return notStorable.Data
        End Function

        ' This is called when the object is activated
        Public Sub OnActivate(ByVal objectContainer As IObjectContainer, _
                              ByVal targetObject As Object, ByVal storedObject As Object) _
                          Implements IObjectConstructor.OnActivate

            Dim notStorable As NonStorableType = DirectCast(targetObject, NonStorableType)
            notStorable.Data = DirectCast(storedObject, String)
        End Sub

        ' Tell db4o which type we use to store the data
        Public Function StoredClass() As Type _
            Implements IObjectConstructor.StoredClass
            
            Return GetType(String)
        End Function

        ' This method is called when a new instance is needed
        Public Function OnInstantiate(ByVal objectContainer As IObjectContainer, _
                                      ByVal storedObject As Object) As Object _
            Implements IObjectConstructor.OnInstantiate
            Return New NonStorableType("")
        End Function
    End Class
    ' #end example

    '''
    ''' This is our example class which represents a not storable type
    '''
    Friend Class NonStorableType
        Private m_data As String
        <Transient()> _
        Private m_dataLength As Integer = 0

        Public Sub New(ByVal data As String)
            Me.m_data = data
            Me.m_dataLength = data.Length
        End Sub

        Public Property Data() As String
            Get
                Return m_data
            End Get
            Set(ByVal value As String)
                m_data = value
            End Set
        End Property

        Public Property DataLength() As Integer
            Get
                Return m_dataLength
            End Get
            Set(ByVal value As Integer)
                m_dataLength = value
            End Set
        End Property
    End Class
End Namespace
