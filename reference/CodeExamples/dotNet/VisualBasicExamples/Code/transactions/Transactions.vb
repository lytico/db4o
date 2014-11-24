Imports Db4objects.Db4o

Namespace Db4oDoc.Code.Transactions
    Public Class Transactions
        Public Shared Sub Main(ByVal args As String())
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("database.db4o")
                CommitChanges(container)
                RollbackChanges(container)
                RefreshAfterRollback(container)
            End Using
        End Sub

        Private Shared Sub RollbackChanges(ByVal container As IObjectContainer)
            ' #example: Commit changes
            container.Store(New Pilot("John"))
            container.Store(New Pilot("Joanna"))

            container.Commit()
            ' #end example
        End Sub

        Private Shared Sub CommitChanges(ByVal container As IObjectContainer)
            ' #example: Rollback changes
            container.Store(New Pilot("John"))
            container.Store(New Pilot("Joanna"))

            container.Rollback()
            ' #end example
        End Sub

        Private Shared Sub RefreshAfterRollback(ByVal container As IObjectContainer)
            ' #example: Refresh objects after rollback
            Dim pilot As Pilot = container.Query(Of Pilot)()(0)
            pilot.Name = "New Name"
            container.Store(pilot)
            container.Rollback()

            ' use refresh to return the in memory objects back
            ' to the state in the database.
            container.Ext().Refresh(pilot, Integer.MaxValue)
            ' #end example
        End Sub
    End Class


    Friend Class Pilot
        Private m_name As String

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
End Namespace
