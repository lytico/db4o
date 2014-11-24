Imports System.Threading
Imports Db4objects.Db4o

Namespace Db4oDoc.Code.Concurrency.Transactions
    Public Class DatabaseSupportWithReadWriteLock
        Implements IDisposable
        Private ReadOnly transactionLock As New ReaderWriterLockSlim()
        Private ReadOnly database As IObjectContainer

        Public Sub New(database As IObjectContainer)
            Me.database = database
        End Sub

        ' #example: The read transaction method
        Public Function InWriteTransaction(Of T)(transactionClosure As TransactionFunction(Of T)) As T
            transactionLock.EnterWriteLock()
            Try
                Return transactionClosure(database)
            Catch generatedExceptionName As Exception
                database.Rollback()
                Throw
            Finally
                database.Commit()
                transactionLock.ExitWriteLock()
            End Try
        End Function
        ' #end example
        ' #example: The write transaction method
        Public Function InReadTransaction(Of T)(transactionClosure As TransactionFunction(Of T)) As T
            transactionLock.EnterReadLock()
            Try
                Return transactionClosure(database)
            Catch generatedExceptionName As Exception
                database.Rollback()
                Throw
            Finally
                database.Commit()
                transactionLock.ExitReadLock()
            End Try
        End Function
        ' #end example

        Public Sub InWriteTransaction(transactionClosure As TransactionAction)
            Const voidReturn As Integer = 0
            InWriteTransaction(Function(container)
                                   transactionClosure(container)
                                   Return voidReturn

                               End Function)
        End Sub

        Public Sub InReadTransaction(transactionClosure As TransactionAction)
            Const voidReturn As Integer = 0
            InReadTransaction(Function(container)
                                  transactionClosure(container)
                                  Return voidReturn

                              End Function)
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            database.Dispose()
        End Sub
    End Class
End Namespace
