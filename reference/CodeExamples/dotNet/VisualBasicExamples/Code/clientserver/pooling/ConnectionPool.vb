Imports System.Collections.Generic
Imports Db4objects.Db4o

Namespace Db4oDoc.Code.ClientServer.Pooling
    Public Delegate Function ClientConnectionFactory() As IObjectContainer

    Public Class ConnectionPool
        Private ReadOnly connectionFactory As ClientConnectionFactory
        Private ReadOnly availableClients As New Queue(Of IObjectContainer)()
        Private ReadOnly leasedClients As IDictionary(Of IObjectContainer, IObjectContainer) = New Dictionary(Of IObjectContainer, IObjectContainer)()
        Private ReadOnly sync As New Object()

        Public Sub New(ByVal connectionFactory As ClientConnectionFactory)
            Me.connectionFactory = connectionFactory
        End Sub

        ''' <summary>
        ''' Get a object-container from the pool. This pool either uses an existing client
        ''' or opens a new connection. The connection is opened with the
        ''' given <see cref="ClientConnectionFactory"/> 
        ''' </summary>
        ''' <returns>A object container from the pool</returns>
        Public Function Acquire() As IObjectContainer
            SyncLock sync
                Dim containerPair As SessionClientPair = AcquirePooledContainer()
                leasedClients.Add(containerPair.SessionContainer, containerPair.ClientContainer)
                Return containerPair.SessionContainer
            End SyncLock
        End Function

        ''' <summary>
        ''' Return the object container to the pool. If you haven't committed the changes yet
        ''' they changes will be lost. Use <see cref="IObjectContainer.Close"/> or <see cref="IObjectContainer.Commit"/>
        ''' before using this method to commit the changes
        ''' </summary>
        ''' <param name="session">The container to return to the pool</param>
        Public Sub Release(ByVal session As IObjectContainer)
            SyncLock sync
                EnsureIsLegitimateContainer(session)
                ReturnToPool(session)
            End SyncLock
        End Sub

        ''' <summary>
        ''' Close and return the object-container. Will commit the changes first and the
        ''' return the object container to the pool
        ''' </summary>
        ''' <param name="session">The container to return to the pool</param>
        Public Sub CloseAndRelease(ByVal session As IObjectContainer)
            session.Dispose()
            Release(session)
        End Sub

        Private Function AcquirePooledContainer() As SessionClientPair
            ' #example: Obtain a pooled container
            ' Obtain a client container. Either take one from the pool or allocate a new one
            Dim client As IObjectContainer = ObtainClientContainer()
            ' Make sure that the transaction is in clean state
            client.Rollback()
            ' Then create a session on that client container and use it for the database operations.
            ' The client-container is now in use. Ensure that it isn't leased twice.
            Dim sessionContainer As IObjectContainer = client.Ext().OpenSession()
            ' #end example
            Return New SessionClientPair(sessionContainer, client)
        End Function


        Private Function ObtainClientContainer() As IObjectContainer
            If availableClients.Count > 0 Then
                Return availableClients.Dequeue()
            End If
            Return connectionFactory()
        End Function

        Private Sub ReturnToPool(ByVal session As IObjectContainer)
            ' #example: Returning to pool
            ' First you need to get the underlying client for the session
            Dim client As IObjectContainer = ClientForSession(session)
            ' then the client is ready for reuse
            ReturnToThePool(client)
            ' #end example
            leasedClients.Remove(session)
        End Sub

        Private Sub ReturnToThePool(ByVal client As IObjectContainer)
            availableClients.Enqueue(client)
        End Sub

        Private Function ClientForSession(ByVal session As IObjectContainer) As IObjectContainer
            Return leasedClients(session)
        End Function

        Private Sub EnsureIsLegitimateContainer(ByVal container As IObjectContainer)
            If Not leasedClients.ContainsKey(container) Then
                Throw New ArgumentException("You cannot return a container which isn't leased")
            End If
        End Sub

        Private Structure SessionClientPair
            Private ReadOnly m_sessionContainer As IObjectContainer
            Private ReadOnly m_clientContainer As IObjectContainer

            Public Sub New(ByVal sessionContainer As IObjectContainer, ByVal clientContainer As IObjectContainer)
                Me.m_sessionContainer = sessionContainer
                Me.m_clientContainer = clientContainer
            End Sub

            Public ReadOnly Property SessionContainer() As IObjectContainer
                Get
                    Return m_sessionContainer
                End Get
            End Property

            Public ReadOnly Property ClientContainer() As IObjectContainer
                Get
                    Return m_clientContainer
                End Get
            End Property
        End Structure
    End Class

End Namespace