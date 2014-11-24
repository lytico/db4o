Imports System.Collections.Generic
Imports System.IO
Imports System.Threading
Imports Db4objects.Db4o
Imports Db4objects.Db4o.CS
Imports Db4objects.Db4o.CS.Config
Imports Db4objects.Db4o.Events
Imports Db4objects.Db4o.Internal

Namespace Db4oDoc.Code.ClientServer.Refresh
    Public Class RefreshingObjects
        Private Const PortNumber As Integer = 1337
        Private Const UsertNameAndPassword As String = "sa"
        Private Const DatabaseFileName As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            UseEventsToRefreshObjects()
            RefreshOnDemand()
        End Sub

        Private Shared Sub RefreshOnDemand()
            CleanUp()

            Using server As IObjectServer = OpenServer()
                server.GrantAccess(UsertNameAndPassword, UsertNameAndPassword)
                StoreJoeOnOtherClient()

                Dim client As IObjectContainer = OpenClient()
                Dim allPersons As IList(Of Person) = ListAllPersons(client)
                PrintPersons(allPersons)

                UpdateJoeOnOtherClient()

                ' the persons are not in the most current state
                PrintPersons(allPersons)

                ' but you can explicitly refresh the objects
                Refresh(client, allPersons)
                PrintPersons(allPersons)


                WaitForALittleWhile()
            End Using
            CleanUp()
        End Sub

        Private Shared Sub Refresh(ByVal db As IObjectContainer, ByVal allPersons As IList(Of Person))
            For Each objToRefresh As Person In allPersons
                ' #example: refresh a object
                db.Ext().Refresh(objToRefresh, Integer.MaxValue)
                ' #end example
            Next
        End Sub

        Private Shared Sub UseEventsToRefreshObjects()
            CleanUp()

            Using server As IObjectServer = OpenServer()
                server.GrantAccess(UsertNameAndPassword, UsertNameAndPassword)
                StoreJoeOnOtherClient()


                Dim client As IObjectContainer = OpenClient()
                RegisterEvent(client)
                Dim allPersons As List(Of Person) = ListAllPersons(client)
                PrintPersons(allPersons)

                UpdateJoeOnOtherClient()

                ' the events are asynchronously transported over the network
                ' which takes a while
                WaitForALittleWhile()
                PrintPersons(allPersons)


                WaitForALittleWhile()
            End Using
            CleanUp()
        End Sub

        Private Shared Sub RegisterEvent(ByVal container As IObjectContainer)
            ' #example: On the updated-event we refresh the objects
            Dim events As IEventRegistry = EventRegistryFactory.ForObjectContainer(container)
            AddHandler events.Committed, AddressOf HandleUpdate
            ' #end example
        End Sub

        ' #example: The refresh-handler
        Private Shared Sub HandleUpdate(ByVal sender As Object, ByVal args As CommitEventArgs)
            For Each updated As LazyObjectReference In args.Updated
                Dim obj As Object = updated.GetObject()
                args.ObjectContainer().Ext().Refresh(obj, 1)
            Next
        End Sub
        ' #end example

        Private Shared Sub PrintPersons(ByVal allPersons As IList(Of Person))
            For Each person As Person In allPersons
                Console.WriteLine(person)
            Next
        End Sub

        Private Shared Sub StoreJoeOnOtherClient()
            Using client As IObjectContainer = OpenClient()
                client.Store(New Person("Joe"))
            End Using
        End Sub

        Private Shared Sub UpdateJoeOnOtherClient()
            Using container As IObjectContainer = OpenClient()
                Dim persons As IList(Of Person) = container.Query(Of Person)()
                For Each person As Person In persons
                    person.Name = "New " & person.Name
                    container.Store(person)
                Next
            End Using
        End Sub


        Private Shared Function ListAllPersons(ByVal container As IObjectContainer) As List(Of Person)
            Dim persons As IList(Of Person) = container.Query(Of Person)()
            Return New List(Of Person)(persons)
        End Function

        Private Shared Sub WaitForALittleWhile()
            Thread.Sleep(200)
        End Sub

        Private Shared Function OpenClient() As IObjectContainer
            Return Db4oClientServer.OpenClient("localhost", PortNumber, UsertNameAndPassword, UsertNameAndPassword)
        End Function


        Private Shared Function OpenServer() As IObjectServer
            Dim configuration As IServerConfiguration = Db4oClientServer.NewServerConfiguration()
            Return Db4oClientServer.OpenServer(configuration, DatabaseFileName, PortNumber)
        End Function

        Private Shared Sub CleanUp()
            File.Delete(DatabaseFileName)
        End Sub
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

        Public Overrides Function ToString() As String
            Return String.Format("Name: {0}", m_name)
        End Function
    End Class
End Namespace