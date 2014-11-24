Imports Db4objects.Db4o
Imports Db4objects.Db4o.Linq

Namespace Db4oDoc.Code.Concurrency.Transactions
    Public Class TransactionOperations
        Public Shared Sub Main(args As String())
            Dim lockOps = New TransactionOperations()
            lockOps.Main()
        End Sub

        Public Sub Main()
            Using database = New DatabaseSupport(Db4oEmbedded.OpenFile("database.db4o"))
                storeInitialObjects(database)

                ' Schedule back-ground tasks
                Dim toRun As Action(Of DatabaseSupport) = AddressOf UpdateAllJoes
                Dim waitHandle = toRun.BeginInvoke(database, Nothing, Nothing)

                ' While doing other work
                ListAllPeople(database)

                ' Wait for the task to finish
                toRun.EndInvoke(waitHandle)
            End Using
        End Sub

        ' #example: Use transaction for reading objects
        Private Sub ListAllPeople(dbSupport As DatabaseSupport)
            dbSupport.InTransaction(
                Function(container)
                    Dim result = From p As Person In container Select p
                    For Each person As Person In result
                        Console.WriteLine(person.Name)
                    Next
                End Function)
        End Sub
        ' #end example

        ' #example: Use transaction to update objects
        Private Sub UpdateAllJoes(dbSupport As DatabaseSupport)
            dbSupport.InTransaction(
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

        Private Sub storeInitialObjects(dbSupport As DatabaseSupport)
            dbSupport.InTransaction(
                Function(container)
                    container.Store(New Person("Joe"))
                    container.Store(New Person("Jan"))
                    container.Store(New Person("Joanna"))
                    container.Store(New Person("Phil"))
                End Function)
        End Sub
    End Class
End Namespace
