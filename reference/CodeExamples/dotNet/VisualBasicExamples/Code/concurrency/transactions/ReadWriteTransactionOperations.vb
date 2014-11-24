Imports Db4objects.Db4o
Imports Db4objects.Db4o.Linq

Namespace Db4oDoc.Code.Concurrency.Transactions
    Public Class ReadWriteTransactionOperations
        Public Shared Sub Main(args As String())
            Dim lockOps = New ReadWriteTransactionOperations()
            lockOps.Main()
        End Sub

        Public Sub Main()
            Using database = New DatabaseSupportWithReadWriteLock(Db4oEmbedded.OpenFile("database.db4o"))
                storeInitialObjects(database)

                ' Schedule back-ground tasks
                Dim toRun As Action(Of DatabaseSupportWithReadWriteLock) = AddressOf UpdateAllJoes
                Dim waitHandle = toRun.BeginInvoke(database, Nothing, Nothing)

                ' While doing other work
                ListAllPeople(database)

                ' Wait for the task to finish
                toRun.EndInvoke(waitHandle)
            End Using
        End Sub

        ' #example: Use a read transaction for reading objects
        Private Sub ListAllPeople(dbSupport As DatabaseSupportWithReadWriteLock)
            dbSupport.InReadTransaction(
                Function(container)
                    Dim result = From p As Person In container Select p
                    For Each person As Person In result
                        Console.WriteLine(person.Name)
                    Next

                End Function)
        End Sub
        ' #end example

        ' #example: Use a write transaction to update objects
        Private Sub UpdateAllJoes(dbSupport As DatabaseSupportWithReadWriteLock)
            dbSupport.InWriteTransaction(
                         Function(container)
                             Dim allJoes = From p As Person In container _
                                         Where p.Name = "Joe" _
                                         Select p
                             For Each joe As Person In allJoes
                                 joe.Name = "New Joe"
                                 container.Store(joe)
                             Next
                         End Function)
        End Sub
        ' #end example

        Private Sub storeInitialObjects(dbSupport As DatabaseSupportWithReadWriteLock)
            dbSupport.InWriteTransaction(Function(container)
                                             container.Store(New Person("Joe"))
                                             container.Store(New Person("Jan"))
                                             container.Store(New Person("Joanna"))
                                             container.Store(New Person("Phil"))

                                         End Function)
        End Sub
    End Class
End Namespace
