Imports Db4objects.Db4o
Imports Db4objects.Db4o.CS
Imports Db4objects.Db4o.CS.Config
Imports Db4objects.Db4o.IO
Imports NUnit.Framework

Namespace Db4oDoc.Code.ClientServer.Pooling
    <TestFixture()> _
    Public Class TestConnectionPool
        Private Const Port As Integer = 1337
        Private Const UserAndPassword As String = "sa"

        Private server As IObjectServer
        Private toTest As ConnectionPool
        Private assertConnectionFactory As AssertionConnectionFactory

        <SetUp()> _
        Public Sub Setup()
            server = CreateInMemoryServer()
            StoreTestObjects()
            assertConnectionFactory = NewFactory()
            toTest = New ConnectionPool(AddressOf assertConnectionFactory.Connect)
        End Sub


        <TearDown()> _
        Public Sub TearDown()
            server.Close()
        End Sub

        <Test()> _
        Public Sub ReturnsConnection()
            Dim toTest As New ConnectionPool(AddressOf NewFactory().Connect)
            Dim container As IObjectContainer = toTest.Acquire()
            Assert.NotNull(container)
        End Sub

        <Test()> _
        Public Sub CreatesConnectionForSecondAquire()
            Dim container1 As IObjectContainer = toTest.Acquire()
            Dim container2 As IObjectContainer = toTest.Acquire()
            assertConnectionFactory.AssertWasCalledTimes(2)
        End Sub


        <Test()> _
        Public Sub CloseAndRelease()
            Dim container1 As IObjectContainer = toTest.Acquire()
            container1.Store(New AStoredObject())
            toTest.CloseAndRelease(container1)
            Assert.AreEqual(2, server.OpenClient().Query(Of AStoredObject)().Count)
        End Sub

        <Test()> _
        Public Sub ReusesContainerIfReleased()
            Dim container1 As IObjectContainer = toTest.Acquire()
            toTest.Release(container1)
            Dim container2 As IObjectContainer = toTest.Acquire()
            assertConnectionFactory.AssertWasCalledTimes(1)
        End Sub

        <Test()> _
        <ExpectedException(GetType(ArgumentException))> _
        Public Sub CannotMultipleReturnClient()
            Dim container1 As IObjectContainer = toTest.Acquire()
            toTest.Release(container1)
            toTest.Release(container1)
        End Sub

        <Test()> _
        Public Sub ReusedClientIsNotTainted()
            Dim container1 As IObjectContainer = toTest.Acquire()
            Dim fromContainer1 As AStoredObject = TestObjectFromContainer(container1)
            toTest.Release(container1)
            Dim fromContainer2 As AStoredObject = TestObjectFromContainer(toTest.Acquire())
            Assert.AreNotSame(fromContainer1, fromContainer2)
        End Sub

        <Test()> _
        Public Sub NowGhostCommits()
            Dim container1 As IObjectContainer = toTest.Acquire()
            container1.Store(New AStoredObject())
            toTest.Release(container1)
            Dim container2 As IObjectContainer = toTest.Acquire()
            container2.Commit()

            Dim countStoredObjects As Integer = server.OpenClient().Query(Of AStoredObject)().Count
            Assert.AreEqual(1, countStoredObjects)
        End Sub

        <Test()> _
        Public Sub CanPassClosedContainers()
            Dim container As IObjectContainer = toTest.Acquire()
            container.Close()
            toTest.Release(container)
        End Sub


        <Test()> _
        Public Sub NoTransactionSharing()
            Dim session1 As IObjectContainer = toTest.Acquire()
            Dim session2 As IObjectContainer = toTest.Acquire()
            session1.Store(New AStoredObject())
            Dim count1 As Integer = session1.Query(Of AStoredObject)().Count
            session2.Rollback()

            Dim count2 As Integer = session1.Query(Of AStoredObject)().Count
            Assert.AreEqual(2, count1)
            Assert.AreEqual(2, count2)
        End Sub

        Private Shared Function TestObjectFromContainer(ByVal container1 As IObjectContainer) As AStoredObject
            Return container1.Query(Of AStoredObject)()(0)
        End Function


        Private Sub StoreTestObjects()
            Dim objectContainer As IObjectContainer = server.OpenClient()
            objectContainer.Store(New AStoredObject())
            objectContainer.Close()
        End Sub

        Private Shared Function NewFactory() As AssertionConnectionFactory
            Return New AssertionConnectionFactory()
        End Function

        Private Class AssertionConnectionFactory
            Private wasCalled As Integer

            Public Function Connect() As IObjectContainer
                wasCalled += 1
                Return Db4oClientServer.OpenClient("localhost", Port, UserAndPassword, UserAndPassword)
            End Function


            Public Sub AssertWasCalledTimes(ByVal times As Integer)
                Assert.AreEqual(times, wasCalled)
            End Sub
        End Class


        Private Shared Function CreateInMemoryServer() As IObjectServer
            Dim config As IServerConfiguration = Db4oClientServer.NewServerConfiguration()
            config.File.Storage = New MemoryStorage()
            Dim server As IObjectServer = Db4oClientServer.OpenServer(config, "In:Memory", Port)
            server.GrantAccess(UserAndPassword, UserAndPassword)
            Return server
        End Function


        Private Class AStoredObject
        End Class
    End Class
End Namespace