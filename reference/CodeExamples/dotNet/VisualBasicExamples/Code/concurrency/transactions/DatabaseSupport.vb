Imports Db4objects.Db4o

Namespace Db4oDoc.Code.Concurrency.Transactions
    Public Delegate Sub TransactionAction(container As IObjectContainer)

    Public Delegate Function TransactionFunction(Of Out T)(container As IObjectContainer) As T

    Public Class DatabaseSupport
        Implements IDisposable
        Private ReadOnly transactionLock As New Object()
        Private ReadOnly database As IObjectContainer

        Public Sub New(database As IObjectContainer)
            Me.database = database
        End Sub

        ' #example: A transaction method
        Public Function InTransaction(Of T)(transactionClosure As TransactionFunction(Of T)) As T
            SyncLock transactionLock
                Try
                    Return transactionClosure(database)
                Catch generatedExceptionName As Exception
                    database.Rollback()
                    Throw
                Finally
                    database.Commit()
                End Try
            End SyncLock
        End Function
        ' #end example

        Public Sub InTransaction(transactionClosure As TransactionAction)
            Const voidReturn As Integer = 0
            InTransaction(Function(container)
                              transactionClosure(container)
                              Return voidReturn

                          End Function)
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            database.Dispose()
        End Sub
    End Class
End Namespace
