Imports System
Imports System.Collections.Generic
Imports System.IO
Imports Db4objects.Drs.Db4o
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Drs

Namespace Db4oDoc.Drs.Db4o
    Public Class Db4oReplicationExamples
        Public Const DesktopDatabaseName As String = "desktopDatabase.db4o"
        Public Const MobileDatabaseName As String = "mobileDatabase.db4o"

        Public Shared Sub Main(ByVal args As String())
            OneWayReplicationExample()

            BiDirectionalReplicationExample()

            SelectiveReplicationByClass()
            SelectiveReplicationWithCondition()
            SelectiveReplicationWithQuery()

            DeletionsReplication()
        End Sub

        Private Shared Sub OneWayReplicationExample()
            DeleteDatabases()
            StoreObjectsIn(DesktopDatabaseName)

            '#example: Prepare unidirectional replication
            Dim desktopDatabase As IObjectContainer = OpenDatabase(DesktopDatabaseName)
            Dim mobileDatabase As IObjectContainer = OpenDatabase(MobileDatabaseName)

            Dim dektopReplicationProvider As IReplicationProvider _
                = New Db4oEmbeddedReplicationProvider(desktopDatabase)
            Dim mobileReplicationProvider As IReplicationProvider _
                = New Db4oEmbeddedReplicationProvider(mobileDatabase)

            Dim replicationSession As IReplicationSession _
                = Replication.Begin(dektopReplicationProvider, mobileReplicationProvider)
            ' set the replication-direction from the desktop database to the mobile database. 
            replicationSession.SetDirection(replicationSession.ProviderA(), replicationSession.ProviderB())
            '#end example

            '#example: One direction replication
            Dim changes As IObjectSet = replicationSession.ProviderA().ObjectsChangedSinceLastReplication()
            For Each changedObject As Object In changes
                replicationSession.Replicate(changedObject)
            Next
            replicationSession.Commit()
            '#end example

            PrintCars(mobileDatabase)

            CloseDBs(desktopDatabase, mobileDatabase)
        End Sub

        Private Shared Sub BiDirectionalReplicationExample()
            DeleteDatabases()
            StoreObjectsIn(DesktopDatabaseName)
            StoreObjectsIn(MobileDatabaseName)

            ' #example: Prepare bidirectional replication
            Dim desktopDatabase As IObjectContainer = OpenDatabase(DesktopDatabaseName)
            Dim mobileDatabase As IObjectContainer = OpenDatabase(MobileDatabaseName)

            Dim dektopReplicationProvider As IReplicationProvider _
                = New Db4oEmbeddedReplicationProvider(desktopDatabase)
            Dim mobileReplicationProvider As IReplicationProvider _
                = New Db4oEmbeddedReplicationProvider(mobileDatabase)

            Dim replicationSession As IReplicationSession _
                = Replication.Begin(dektopReplicationProvider, mobileReplicationProvider)
            ' #end example

            '#example: Bidirectional replication
            ' First get the changes of the two replication-partners
            Dim changesOnDesktop As IObjectSet _
                = replicationSession.ProviderA().ObjectsChangedSinceLastReplication()
            Dim changesOnMobile As IObjectSet _
                = replicationSession.ProviderB().ObjectsChangedSinceLastReplication()

            ' then iterate over both change-sets and replicate it
            For Each changedObjectOnDesktop As Object In changesOnDesktop
                replicationSession.Replicate(changedObjectOnDesktop)
            Next

            For Each changedObjectOnMobile As Object In changesOnMobile
                replicationSession.Replicate(changedObjectOnMobile)
            Next

            replicationSession.Commit()
            '#end example

            PrintCars(mobileDatabase)
            PrintCars(desktopDatabase)

            CloseDBs(desktopDatabase, mobileDatabase)
        End Sub

        Private Shared Sub SelectiveReplicationByClass()
            DeleteDatabases()
            StoreObjectsIn(DesktopDatabaseName)

            Dim desktopDatabase As IObjectContainer = OpenDatabase(DesktopDatabaseName)
            Dim mobileDatabase As IObjectContainer = OpenDatabase(MobileDatabaseName)

            Dim dektopReplicationProvider As IReplicationProvider _
                = New Db4oEmbeddedReplicationProvider(desktopDatabase)
            Dim mobileReplicationProvider As IReplicationProvider _
                = New Db4oEmbeddedReplicationProvider(mobileDatabase)

            Dim replicationSession As IReplicationSession _
                = Replication.Begin(dektopReplicationProvider, mobileReplicationProvider)

            ' #example: Selective replication by class
            Dim changesOnDesktop As IObjectSet = _
                replicationSession.ProviderA().ObjectsChangedSinceLastReplication(GetType(Pilot))

            For Each changedObjectOnDesktop As Object In changesOnDesktop
                replicationSession.Replicate(changedObjectOnDesktop)
            Next

            replicationSession.Commit()
            ' #end example

            ' the car's aren't replicated, only the pilots
            PrintCars(mobileDatabase)
            PrintPilots(mobileDatabase)

            CloseDBs(desktopDatabase, mobileDatabase)
        End Sub

        Private Shared Sub SelectiveReplicationWithCondition()
            DeleteDatabases()
            StoreObjectsIn(DesktopDatabaseName)

            Dim desktopDatabase As IObjectContainer = OpenDatabase(DesktopDatabaseName)
            Dim mobileDatabase As IObjectContainer = OpenDatabase(MobileDatabaseName)

            Dim dektopReplicationProvider As IReplicationProvider _
                = New Db4oEmbeddedReplicationProvider(desktopDatabase)
            Dim mobileReplicationProvider As IReplicationProvider _
                = New Db4oEmbeddedReplicationProvider(mobileDatabase)

            Dim replicationSession As IReplicationSession _
                = Replication.Begin(dektopReplicationProvider, mobileReplicationProvider)

            ' #example: Selective replication with a condition
            Dim changesOnDesktop As IObjectSet = _
                replicationSession.ProviderA().ObjectsChangedSinceLastReplication()

            For Each changedObjectOnDesktop As Object In changesOnDesktop
                If TypeOf changedObjectOnDesktop Is Car Then
                    If DirectCast(changedObjectOnDesktop, Car).Name.StartsWith("M") Then
                        replicationSession.Replicate(changedObjectOnDesktop)
                    End If
                End If
            Next

            replicationSession.Commit()
            ' #end example

            ' now only the cars which names start with "M" are replicated
            PrintCars(mobileDatabase)

            CloseDBs(desktopDatabase, mobileDatabase)
        End Sub

        Private Shared Sub SelectiveReplicationWithQuery()
            DeleteDatabases()
            StoreObjectsIn(DesktopDatabaseName)

            Dim desktopDatabase As IObjectContainer = OpenDatabase(DesktopDatabaseName)
            Dim mobileDatabase As IObjectContainer = OpenDatabase(MobileDatabaseName)

            Dim dektopReplicationProvider As IReplicationProvider _
                = New Db4oEmbeddedReplicationProvider(desktopDatabase)
            Dim mobileReplicationProvider As IReplicationProvider _
                = New Db4oEmbeddedReplicationProvider(mobileDatabase)

            Dim replicationSession As IReplicationSession _
                = Replication.Begin(dektopReplicationProvider, mobileReplicationProvider)

            ' #example: Selective replication with a query
            Dim changesOnDesktop As IList(Of Car) = _
                desktopDatabase.Query(Function(car As Car) car.Name.StartsWith("M"))

            For Each changedObjectOnDesktop As Car In changesOnDesktop
                replicationSession.Replicate(changedObjectOnDesktop)
            Next

            replicationSession.Commit()
            ' #end example

            ' now only the cars which names start with "M" are replicated
            PrintCars(mobileDatabase)

            CloseDBs(desktopDatabase, mobileDatabase)
        End Sub

        Private Shared Sub DeletionsReplication()
            DeleteDatabases()
            StoreObjectsIn(DesktopDatabaseName)
            Dim desktopDatabase As IObjectContainer = OpenDatabase(DesktopDatabaseName)
            Dim mobileDatabase As IObjectContainer = OpenDatabase(MobileDatabaseName)

            Replicate(desktopDatabase, mobileDatabase)

            Dim carToDelete As Car = desktopDatabase.Query(Of Car)()(0)
            desktopDatabase.Delete(carToDelete)
            desktopDatabase.Commit()

            PrintCars(mobileDatabase)

            ' #example: Replicate deletions
            Dim replicationSession As IReplicationSession = _
                Replication.Begin(desktopDatabase, mobileDatabase)
            replicationSession.ReplicateDeletions(GetType(Car))
            replicationSession.Commit()
            ' #end example

            PrintCars(mobileDatabase)

            CloseDBs(desktopDatabase, mobileDatabase)
        End Sub

        Private Shared Sub Replicate(ByVal desktopDatabase As IObjectContainer, ByVal mobileDatabase As IObjectContainer)
            Dim dektopReplicationProvider As IReplicationProvider _
                = New Db4oEmbeddedReplicationProvider(desktopDatabase)
            Dim mobileReplicationProvider As IReplicationProvider _
                = New Db4oEmbeddedReplicationProvider(mobileDatabase)

            Dim replicationSession As IReplicationSession _
                = Replication.Begin(dektopReplicationProvider, mobileReplicationProvider)
            ReplicateChanges(replicationSession, replicationSession.ProviderA())
            ReplicateChanges(replicationSession, replicationSession.ProviderB())
            replicationSession.Commit()
        End Sub

        Private Shared Sub ReplicateChanges(ByVal replication As IReplicationSession, ByVal provider As IReplicationProvider)
            Dim changes As IObjectSet = provider.ObjectsChangedSinceLastReplication()
            For Each changedObject As Object In changes
                replication.Replicate(changedObject)
            Next
        End Sub

        Private Shared Sub CloseDBs(ByVal ParamArray databases As IObjectContainer())
            For Each db As IObjectContainer In databases
                db.Dispose()
            Next
        End Sub

        Private Shared Sub PrintCars(ByVal database As IObjectContainer)
            Dim cars As IList(Of Car) = database.Query(Of Car)()
            For Each car As Car In cars
                Console.WriteLine(car)
            Next
        End Sub

        Private Shared Sub PrintPilots(ByVal database As IObjectContainer)
            Dim pilots As IList(Of Pilot) = database.Query(Of Pilot)()
            For Each pilot As Pilot In pilots
                Console.WriteLine(pilot)
            Next
        End Sub


        Private Shared Sub StoreObjectsIn(ByVal databaseFile As String)
            Using db As IObjectContainer = OpenDatabase(databaseFile)
                Dim john As New Pilot("John", 100)
                Dim johnsCar As New Car(john, "John's Car")
                db.Store(johnsCar)
                Dim max As New Pilot("Max", 200)
                Dim maxsCar As New Car(max, "Max's Car")
                db.Store(maxsCar)
                db.Commit()
            End Using
        End Sub

        Private Shared Function OpenDatabase(ByVal fileName As String) As IObjectContainer
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            ' #example: Configure db4o to generate UUIDs and commit timestamps
            configuration.File.GenerateUUIDs = ConfigScope.Globally
            configuration.File.GenerateCommitTimestamps = True
            ' #end example
            Return Db4oEmbedded.OpenFile(configuration, fileName)
        End Function

        Private Shared Sub DeleteDatabases()
            File.Delete(DesktopDatabaseName)
            File.Delete(MobileDatabaseName)
        End Sub
    End Class

    Public Class Pilot
        Private m_name As String
        Private m_points As Integer

        Public Sub New(ByVal name As String)
            Me.m_name = name
        End Sub

        Public Sub New(ByVal name As String, ByVal points As Integer)
            Me.m_name = name
            Me.m_points = points
        End Sub

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                m_name = value
            End Set
        End Property

        Public Property Points() As Integer
            Get
                Return m_points
            End Get
            Set(ByVal value As Integer)
                m_points = value
            End Set
        End Property

        Public Overloads Overrides Function ToString() As String
            Return m_name
        End Function
    End Class

    Public Class Car
        Private m_pilot As Pilot
        Private m_name As String

        Public Sub New(ByVal pilot As Pilot, ByVal name As String)
            Me.m_pilot = pilot
            Me.m_name = name
        End Sub

        Public Property Pilot() As Pilot
            Get
                Return m_pilot
            End Get
            Set(ByVal value As Pilot)
                m_pilot = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                m_name = value
            End Set
        End Property

        Public Overloads Overrides Function ToString() As String
            Return m_name & " pilot: " & m_pilot.ToString
        End Function
    End Class
End Namespace
