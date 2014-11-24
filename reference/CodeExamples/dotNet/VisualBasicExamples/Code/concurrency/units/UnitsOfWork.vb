Imports Db4objects.Db4o
Imports Db4objects.Db4o.Linq

Namespace Db4oDoc.Code.concurrency.UnitsOfWork
    Public Class UnitsOfWork
        Public Shared Sub Main(args As String())
            Dim lockOps = New UnitsOfWork()
            lockOps.Main()
        End Sub

        Public Sub Main()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("database.db4o")
                StoreInitialObjects(container)

                ' #example: Schedule back-ground tasks
                ' Schedule back-ground tasks
                Dim toRun As Action(Of IObjectContainer) = AddressOf UpdateSomePeople
                Dim waitHandle = toRun.BeginInvoke(container, Nothing, Nothing)

                ' While doing other work
                ListAllPeople(container)
                ' #end example

                ' Wait for the tasks to finish
                toRun.EndInvoke(waitHandle)
            End Using
        End Sub

        ' #example: An object container for this unit of work
        Private Sub ListAllPeople(rootContainer As IObjectContainer)
            Using container As IObjectContainer = rootContainer.Ext().OpenSession()
                For Each person As Person In From p In container Select p
                    Console.WriteLine(person.Name)
                Next
            End Using
        End Sub
        ' #end example

        ' #example: An object container for the background task
        Private Sub UpdateSomePeople(rootContainer As IObjectContainer)
            Using container As IObjectContainer = rootContainer.Ext().OpenSession()
                Dim people = From p As Person In container Where p.Name.Equals("Joe") Select p
                For Each joe As Person In people
                    joe.Name = "New Joe"
                    container.Store(joe)
                Next
            End Using
        End Sub
        ' #end example:

        Private Sub StoreInitialObjects(rootContainer As IObjectContainer)
            Using container As IObjectContainer = rootContainer.Ext().OpenSession()
                container.Store(New Person("Joe"))
                container.Store(New Person("Jan"))
                container.Store(New Person("Joanna"))
                container.Store(New Person("Phil"))
            End Using
        End Sub
    End Class


    Friend Class Person
        Private m_name As String

        Public Sub New(name As String)
            Me.m_name = name
        End Sub

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(value As String)
                m_name = value
            End Set
        End Property
    End Class
End Namespace
