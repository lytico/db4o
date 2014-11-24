Imports System
Imports System.Collections.Generic
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Events

Namespace Db4oDoc.Code.DisconnectedObj.IdExamples
    Public Class AutoIncrement
        Private state As PersistedAutoIncrements = Nothing
        Private ReadOnly container As IObjectContainer
        Private ReadOnly dataLock As New Object()

        Public Sub New(ByVal container As IObjectContainer)
            Me.container = container
        End Sub


        ' #example: getting the next id and storing state
        Public Function GetNextID(ByVal forClass As Type) As Integer
            SyncLock dataLock
                Dim incrementState As PersistedAutoIncrements = EnsureLoadedIncrements()
                Return incrementState.NextNumber(forClass)
            End SyncLock
        End Function

        Public Sub StoreState()
            SyncLock dataLock
                If state IsNot Nothing Then
                    container.Ext().Store(state, 2)
                End If
            End SyncLock
        End Sub

        ' #end example

        ' #example: load the state from the database
        Private Function EnsureLoadedIncrements() As PersistedAutoIncrements
            If state Is Nothing Then
                state = LoadOrCreateState()
            End If
            Return state
        End Function

        Private Function LoadOrCreateState() As PersistedAutoIncrements
            Dim existingState As IList(Of PersistedAutoIncrements) = container.Query(Of PersistedAutoIncrements)()
            If 0 = existingState.Count Then
                Return New PersistedAutoIncrements()
            ElseIf 1 = existingState.Count Then
                Return existingState(0)
            Else
                Throw New InvalidOperationException("Cannot have more than one state stored in database")
            End If
        End Function


        ' #end example

        Public Sub HandleCreating(ByVal sender As Object, ByVal e As CancellableObjectEventArgs)
            If TypeOf e.Object Is IDHolder Then
                Dim idHolder As IDHolder = DirectCast(e.Object, IDHolder)
                idHolder.Id = GetNextID(idHolder.GetType())
            End If
        End Sub

        Public Sub HandleCommiting(ByVal sender As Object, ByVal e As CommitEventArgs)
            container.Store(Me)
        End Sub

        ' #example: persistent auto increment
        Private Class PersistedAutoIncrements
            Private ReadOnly currentHighestIds As IDictionary(Of Type, Integer) _
                                = New Dictionary(Of Type, Integer)()

            Public Function NextNumber(ByVal forClass As Type) As Integer
                Dim number As Integer
                If Not currentHighestIds.TryGetValue(forClass, number) Then
                    number = 0
                End If
                number += 1
                currentHighestIds(forClass) = number
                Return number
            End Function
        End Class

        ' #end example

    End Class
End Namespace
