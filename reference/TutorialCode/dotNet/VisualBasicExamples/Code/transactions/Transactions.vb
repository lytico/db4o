Imports System.Linq
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Linq
Imports Db4objects.Db4o.TA

Namespace Db4oTutorialCode.Code.Transactions
    Public Class Transactions
        Private Const DatabaseFile As String = "database.db4o"

        Public Shared Sub Main(args As String())
            StoreExampleObjects()
            CommitTransactions()
            RollbackTransactions()
            ObjectStateAfterRollbackWithoutTp()
            ObjectStateAfterRollbackWithTp()
            MultipleTransactions()
        End Sub

        Private Shared Sub CommitTransactions()
            Using container As IObjectContainer = OpenDatabase()
                Dim toyota = New Car("Toyota Corolla")
                Dim jimmy = New Driver("Jimmy", toyota)
                container.Store(jimmy)
                ' #example: Committing changes
                container.Commit()
                ' #end example
            End Using
        End Sub

        Private Shared Sub RollbackTransactions()
            Using container As IObjectContainer = OpenDatabase()
                Dim toyota = New Car("Toyota Corolla")
                Dim jimmy = New Driver("Jimmy", toyota)
                container.Store(jimmy)
                ' #example: Rollback changes
                container.Rollback()
                ' #end example
            End Using
        End Sub

        Private Shared Sub ObjectStateAfterRollbackWithoutTp()
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
            If True Then
                ' #example: Without transparent persistence objects in memory aren't rolled back
                Dim driver As Driver = QueryForDriver(container)
                driver.Name = "New Name"
                Console.WriteLine("Name before rollback {0}", driver.Name)
                container.Rollback()
                ' Without transparent persistence objects keep the state in memory
                Console.WriteLine("Name after rollback {0}", driver.Name)
                ' After refreshing the object is has the state like in the database
                container.Ext().Refresh(driver, Integer.MaxValue)
                Console.WriteLine("Name after rollback {0}", driver.Name)
                ' #end example
            End If
        End Sub

        Private Shared Sub ObjectStateAfterRollbackWithTp()
            Using container As IObjectContainer = OpenDatabase()
                ' #example: Transparent persistence rolls back objects in memory
                Dim driver As Driver = QueryForDriver(container)
                driver.Name = "New Name"
                Console.WriteLine("Name before rollback {0}", driver.Name)
                container.Rollback()
                ' Thanks to transparent persistence with the rollback strategy
                ' the object state is rolled back
                Console.WriteLine("Name after rollback {0}", driver.Name)
                ' #end example
            End Using
        End Sub
        Private Shared Sub MultipleTransactions()
            Using rootContainer As IObjectContainer = OpenDatabase()
                ' #example: Opening a new transaction
                Using container As IObjectContainer = rootContainer.Ext().OpenSession()
                    ' We do our operations in this transaction
                End Using
                ' #end example
            End Using
        End Sub


        Private Shared Sub StoreExampleObjects()
            Using container As IObjectContainer = OpenDatabase()
                Dim vwBeetle = New Car("VW Beetle")
                Dim audi = New Car("Audi A6")
                Dim ferrari = New Car("Ferrari")

                Dim joe = New Driver("Joe", audi)
                Dim joanna = New Driver("Joanna", vwBeetle)
                Dim jenny = New Driver("Jenny")
                Dim john = New Driver("John", ferrari)
                Dim jim = New Driver("Jim", audi)

                container.Store(joe)
                container.Store(joanna)
                container.Store(jenny)
                container.Store(john)
                container.Store(jim)
            End Using
        End Sub

        Private Shared Function OpenDatabase() As IObjectContainer
            ' #example: Rollback strategy for the transaction
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.Add(New TransparentPersistenceSupport(New DeactivatingRollbackStrategy()))
            ' #end example
            Return Db4oEmbedded.OpenFile(configuration, DatabaseFile)
        End Function

        Private Shared Function QueryForDriver(container As IObjectContainer) As Driver
            Return (From d As Driver In container _
                    Select d).First()
        End Function
    End Class
End Namespace
