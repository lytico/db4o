Imports System
Imports System.IO
Imports System.Threading
Imports Db4objects.Db4o
Imports Db4objects.Db4o.CS
Imports Db4objects.Db4o.Events
Imports Db4objects.Db4o.Internal
Imports Db4objects.Db4o.Linq

Namespace Db4oDoc.Code.Callbacks.EventRegistry
    Public Class EventRegistryExamples
        Private Const DatabaseFileName As String = "database.db4o"
        Private Const PortNumber As Integer = 1337
        Private Const EmbeddedUser As String = "user"
        Private Const EmbeddedPassword As String = "user"

        Public Shared Sub Main(ByVal args As String())
            Console.WriteLine("--Events in embedded mode--")
            EventsInLocalContainer()
            Console.WriteLine("--Events in client/server mode--")
            EventsClientServer()
            Console.WriteLine("--Cancel in event --")
            CancelInEvent()
            Console.WriteLine("--Commit-events --")
            CommitEvents()
        End Sub


        Private Shared Sub EventsInLocalContainer()
            CleanUp()
            StoreJoe()

            Using container As IObjectContainer = OpenEmbedded()
                ' #example: Obtain the event-registry
                Dim events As IEventRegistry = EventRegistryFactory.ForObjectContainer(container)
                ' #end example

                RegisterAFewEvents(events, "local embedded container")
                RunOperations(container)
            End Using
            CleanUp()
        End Sub

        Private Shared Sub RegisterForEventsOnTheServer()
            ' #example: register for events on the server
            Dim server As IObjectServer = _
                    Db4oClientServer.OpenServer(DatabaseFileName, PortNumber)
            Dim eventsOnServer As IEventRegistry = _ 
                    EventRegistryFactory.ForObjectContainer(server.Ext().ObjectContainer())
            ' #end example
        End Sub

        Private Shared Sub EventsClientServer()
            CleanUp()
            StoreJoe()

            Using server As IObjectServer = OpenServer()
                Dim eventsOnServer As IEventRegistry = EventRegistryFactory.ForObjectContainer(server.Ext().ObjectContainer())
                RegisterAFewEvents(eventsOnServer, "db4o server")

                Dim client1 As IObjectContainer = OpenClient()
                Dim eventsOnClient1 As IEventRegistry = EventRegistryFactory.ForObjectContainer(client1)
                RegisterAFewEvents(eventsOnClient1, "db4o client 1")
                RunOperations(client1)


                Dim client2 As IObjectContainer = OpenClient()
                Dim eventsOnClient2 As IEventRegistry = EventRegistryFactory.ForObjectContainer(client2)
                RegisterAFewEvents(eventsOnClient2, "db4o client 2")

                SleepForAWhile()
                client1.Dispose()
                client2.Dispose()
            End Using

            CleanUp()
        End Sub



        ' #example: Cancel store operation Handler
        Private Shared Sub HandleCreatingEvent(ByVal sender As Object, _
             ByVal args As CancellableObjectEventArgs)
            If TypeOf args.Object Is Person Then
                Dim p As Person = DirectCast(args.Object, Person)
                If p.Name.Equals("Joe Junior") Then
                    args.Cancel()
                End If
            End If

        End Sub
        ' #end example

        Private Shared Sub CancelInEvent()
            CleanUp()
            StoreJoe()

            Using container As IObjectContainer = OpenEmbedded()
                ' #example: Cancel store operation
                Dim events As IEventRegistry = EventRegistryFactory.ForObjectContainer(container)
                AddHandler events.Creating, AddressOf HandleCreatingEvent
                ' #end example

                container.Store(New Person("Joe Junior"))

                Dim personCount As Integer = container.Query(Of Person)().Count
                Console.WriteLine("Only " & personCount & " because store was cancelled")
            End Using
            CleanUp()
        End Sub

        Private Shared Sub CommitEvents()
            CleanUp()
            StoreJoe()

            Using container As IObjectContainer = OpenEmbedded()
                ' #example: Commit-info
                Dim events As IEventRegistry = EventRegistryFactory.ForObjectContainer(container)
                AddHandler events.Committed, AddressOf HandlingCommitEvent
                ' #end example
                RunOperations(container)
            End Using
            CleanUp()
        End Sub

        ' #example: Commit-info Handler
        Private Shared Sub HandlingCommitEvent(ByVal sender As Object, ByVal args As CommitEventArgs)
            For Each reference As LazyObjectReference In args.Added
                Console.WriteLine("Added " & reference.GetObject())
            Next
            For Each reference As LazyObjectReference In args.Updated
                Console.WriteLine("Updated " & reference.GetObject())
            Next
            For Each reference As FrozenObjectInfo In args.Deleted
                'the deleted info might doesn't contain the object anymore and
                'return the null.
                Console.WriteLine("Deleted " & reference.GetObject())
            Next
        End Sub
        ' #end example

        Private Shared Sub RunOperations(ByVal container As IObjectContainer)
            Dim joe = (From p As Person In container _
                                Where p.Name.Equals("Joe") _
                                Select p).First()
            joe.Name = "Joe Senior"
            container.Store(joe)
            container.Store(New Person("Joe Junior"))
            container.Commit()
        End Sub

        Private Shared Sub StoreJoe()
            Using container As IObjectContainer = OpenEmbedded()
                container.Store(New Person("Joe"))
            End Using
        End Sub

        Private Shared Sub RegisterAFewEvents(ByVal events As IEventRegistry, ByVal containerName As String)
            AddHandler events.Activating, AddressOf OnEvent("Activating", containerName).HandleEvent
            AddHandler events.Activated, AddressOf OnEvent("Activated", containerName).HandleEvent
            AddHandler events.Creating, AddressOf OnEvent("Creating", containerName).HandleEvent
            AddHandler events.Created, AddressOf OnEvent("Created", containerName).HandleEvent
            AddHandler events.Updating, AddressOf OnEvent("Updating", containerName).HandleEvent
            AddHandler events.Updated, AddressOf OnEvent("Updated", containerName).HandleEvent
            AddHandler events.QueryStarted, AddressOf OnEvent("QueryStarted", containerName).HandleEvent
            AddHandler events.QueryFinished, AddressOf OnEvent("QueryFinished", containerName).HandleEvent
            AddHandler events.Committing, AddressOf OnEvent("Committing", containerName).HandleEvent
            AddHandler events.Committed, AddressOf OnEvent("Committed", containerName).HandleEvent

            ' #example: register for a event
            AddHandler events.Committing, AddressOf HandleCommitting
            ' #end example
        End Sub

        ' #example: implement your event handling
        Private Shared Sub HandleCommitting(ByVal sender As Object, _
                                            ByVal commitEventArgs As CommitEventArgs)
            ' handle the event           
        End Sub
        ' #end example


        Private Shared Function OpenEmbedded() As IObjectContainer
            Return Db4oEmbedded.OpenFile(DatabaseFileName)
        End Function

        Private Shared Function OpenClient() As IObjectContainer
            Return Db4oClientServer.OpenClient("localhost", PortNumber, EmbeddedUser, EmbeddedPassword)
        End Function

        Private Shared Function OpenServer() As IObjectServer
            Dim server As IObjectServer = Db4oClientServer.OpenServer(DatabaseFileName, PortNumber)
            server.GrantAccess(EmbeddedUser, EmbeddedPassword)
            Return server
        End Function

        Private Shared Sub CleanUp()
            File.Delete(DatabaseFileName)
        End Sub

        Private Shared Sub SleepForAWhile()
            Thread.Sleep(2000)
        End Sub

        Private Shared Function OnEvent(ByVal action As String, ByVal name As String) As EventHandler
            Dim handler As EventHandler = New EventHandler(action, name)
            Return handler
        End Function

    End Class

    Public Class Person
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

        Public Overloads Overrides Function ToString() As String
            Return String.Format("Name: {0}", m_name)
        End Function
    End Class



    Friend Class EventHandler
        Private m_containerName As String
        Private m_action As String

        Public Sub New(ByVal action As String, ByVal name As String)
            Me.m_action = action
            Me.m_containerName = name
        End Sub

        Public Sub HandleEvent()
            Console.Out.WriteLine("{0} on {1}", m_action, m_containerName)
        End Sub
    End Class
End Namespace
