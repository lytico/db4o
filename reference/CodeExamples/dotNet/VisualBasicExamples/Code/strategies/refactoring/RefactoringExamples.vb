Imports System.Collections.Generic
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Ext

Namespace Db4oDoc.Code.Strategies.Refactoring
    Public Class RefactoringExamples
        Private Const DatabaseFile As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            RenameFieldAndClass()
            ChangeType()
        End Sub


        Private Shared Sub ChangeType()
            StoreInDB(New Person(), New Person("John"))


            ' #example: copying the data from the old field type to the new one
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("database.db4o")
                ' first get all objects which should be updated
                Dim persons As IList(Of Person) = container.Query(Of Person)()
                For Each person As Person In persons
                    ' get the database-metadata about this object-type
                    Dim dbClass As IStoredClass = container.Ext().StoredClass(person)
                    ' get the old field which was an int-type
                    Dim oldField As IStoredField = dbClass.StoredField("id", GetType(Integer))
                    If oldField IsNot Nothing Then
                        ' Access the old data and copy it to the new field!
                        Dim oldValue As Object = oldField.Get(person)
                        If oldValue IsNot Nothing Then
                            person.id = New Identity(CInt(oldValue))
                            container.Store(person)
                        End If
                    End If
                Next
            End Using
            ' #end example
        End Sub

        Private Shared Sub RenameFieldAndClass()
            CreateOldDatabase()


            Dim configuration As IEmbeddedConfiguration = RefactorClassAndFieldName()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
                Dim persons As IList(Of PersonNew) = container.Query(Of PersonNew)()
                For Each person As PersonNew In persons
                    Console.Out.WriteLine(person.Sirname)
                Next
            End Using
        End Sub

        Private Shared Function RefactorClassAndFieldName() As IEmbeddedConfiguration
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            ' #example: Rename a class
            configuration.Common.ObjectClass("Db4oDoc.Code.Strategies.Refactoring.PersonOld, Db4oDoc") _
                .Rename("Db4oDoc.Code.Strategies.Refactoring.PersonNew, Db4oDoc")
            ' #end example:
            ' #example: Rename field
            configuration.Common.ObjectClass("Db4oDoc.Code.Strategies.Refactoring.PersonOld, Db4oDoc") _
                .ObjectField("name").Rename("sirname")
            ' #end example
            Return configuration
        End Function

        Private Shared Sub CreateOldDatabase()
            StoreInDB(New PersonOld(), New PersonOld("Papa Joe"))
        End Sub

        Private Shared Sub StoreInDB(ByVal ParamArray objects As Object())
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                For Each obj As Object In objects
                    container.Store(obj)
                Next
            End Using
        End Sub
    End Class


    Public Class PersonOld
        Private m_name As String = "Joe"

        Public Sub New()
        End Sub

        Public Sub New(ByVal name As String)
            Me.m_name = name
        End Sub

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                m_name = value
            End Set
        End Property
    End Class

    Public Class PersonNew
        Private m_sirname As String = "Joe"

        Public Property Sirname() As String
            Get
                Return m_sirname
            End Get
            Set(ByVal value As String)
                m_sirname = value
            End Set
        End Property
    End Class

    Friend Class Person
        ' #example: change type of field
        Public m_id As Identity = Identity.NewId()
        '  was an int previously:
        '    public int id = new Random().nextInt();
        ' #end example

        Private m_name As String

        Public Sub New(ByVal name As String)
            Me.m_name = name
        End Sub

        Public Sub New()
            Me.New("Joe")
        End Sub

        Public Property ID() As Identity
            Get
                Return m_id
            End Get
            Set(ByVal value As Identity)
                m_id = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                m_name = value
            End Set
        End Property


        Public Overrides Function ToString() As String
            Return "Person{" & "id=" & Convert.ToString(m_id) & ", name='" & m_name & "'"c & "}"c
        End Function
    End Class

    Friend Class Identity
        Private ReadOnly id As Integer

        Public Sub New(ByVal id As Integer)
            Me.id = id
        End Sub

        Public Shared Function NewId() As Identity
            Return New Identity(New Random().Next())
        End Function

        Public Overrides Function ToString() As String
            Return id.ToString()
        End Function
    End Class
End Namespace