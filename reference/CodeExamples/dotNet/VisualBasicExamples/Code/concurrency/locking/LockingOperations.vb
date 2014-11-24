Imports Db4objects.Db4o
Imports Db4objects.Db4o.Linq

Namespace Db4oDoc.Code.Concurrency.Locking
    Public Class LockingOperations
        Private ReadOnly dataLock As New Object()

        Public Shared Sub Main(args As String())
            Dim lockOps = New LockingOperations()
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

        ' #example: Grab the lock to show the data
        Private Sub ListAllPeople(container As IObjectContainer)
            SyncLock dataLock
                For Each person In (From p As Person In container Select p)
                    Console.WriteLine(person.Name)
                Next
            End SyncLock
        End Sub
        ' #end example

        ' #example: Grab the lock protecting the data
        Private Function UpdateSomePeople(container As IObjectContainer) As Boolean
            SyncLock dataLock
                Dim people = From p As Person In container Where p.Name.Equals("Joe") Select p
                For Each joe As Person In people
                    joe.Name = "New Joe"
                    container.Store(joe)
                Next
            End SyncLock
            Return True
        End Function
        ' #end example:

        Private Sub StoreInitialObjects(container As IObjectContainer)
            SyncLock dataLock
                container.Store(New Person("Joe"))
                container.Store(New Person("Jan"))
                container.Store(New Person("Joanna"))
                container.Store(New Person("Phil"))
            End SyncLock
        End Sub
    End Class
End Namespace
