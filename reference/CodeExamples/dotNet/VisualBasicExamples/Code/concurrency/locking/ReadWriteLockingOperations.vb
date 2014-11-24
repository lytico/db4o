Imports System.Threading
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Linq

Namespace Db4oDoc.Code.Concurrency.Locking
    Public Class ReadWriteLockingOperations
        Private ReadOnly dataLock As New ReaderWriterLockSlim()

        Public Shared Sub Main(args As String())
            Dim lockOps = New LockingOperations()
            lockOps.Main()
        End Sub

        Public Sub Main()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("database.db4o")
                StoreInitialObjects(container)

                ' Schedule back-ground tasks
                Dim toRun As Action(Of IObjectContainer) = AddressOf UpdateSomePeople
                Dim waitHandle = toRun.BeginInvoke(container, Nothing, Nothing)

                ' While doing other work
                ListAllPeople(container)

                ' Wait for the tasks to finish
                toRun.EndInvoke(waitHandle)
            End Using
        End Sub

        ' #example: Grab the read-lock to show the data
        Private Sub ListAllPeople(container As IObjectContainer)
            dataLock.EnterReadLock()
            Try
                For Each person In (From p As Person In container Select p)
                    Console.WriteLine(person.Name)
                Next
            Finally
                dataLock.ExitReadLock()
            End Try
        End Sub
        ' #end example

        ' #example: Grab the write-lock to change the data
        Private Sub UpdateSomePeople(container As IObjectContainer)
            dataLock.EnterWriteLock()
            Try
                Dim people = From p As Person In container Where p.Name.Equals("Joe") Select p
                For Each joe As Person In people
                    joe.Name = "New Joe"
                    container.Store(joe)
                Next
            Finally
                dataLock.ExitWriteLock()
            End Try
        End Sub
        ' #end example:

        Private Sub StoreInitialObjects(container As IObjectContainer)
            dataLock.EnterWriteLock()
            Try
                container.Store(New Person("Joe"))
                container.Store(New Person("Jan"))
                container.Store(New Person("Joanna"))
                container.Store(New Person("Phil"))
            Finally
                dataLock.ExitWriteLock()
            End Try
        End Sub
    End Class
End Namespace
