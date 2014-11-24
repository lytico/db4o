Imports System
Imports System.Collections.Generic
Imports System.IO
Imports Db4objects.Drs.Db4o
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.CS
Imports Db4objects.Db4o.CS.Config
Imports Db4objects.Db4o.Ext
Imports Db4objects.Drs

Namespace Db4oDoc.Drs.Advanced
    Public Class AdvancedReplicationExamples
        Public Const DesktopDatabaseName As String = "desktopDatabase.db4o"
        Public Const MobileDatabaseName As String = "mobileDatabase.db4o"

        Private Const UserName As String = "db4o"
        Private Const Port As Integer = 4242
        Private Const Host As String = "localhost"


        Public Shared Sub Main(ByVal args As String())
            EventExample()

            ReplicationConflicts()
            ReplicationConflictTakeLatestChange()

            ConcurrencyLimitations()

            SimpleMigration()
            MigrationOnTheFly()
        End Sub

        Private Shared Sub EventExample()
            DeleteDatabases()
            StoreObjectsIn(DesktopDatabaseName)

            Dim desktopDatabase As IObjectContainer = OpenDatabase(DesktopDatabaseName)
            Dim mobileDatabase As IObjectContainer = OpenDatabase(MobileDatabaseName)

            Dim dektopReplicationProvider As IReplicationProvider _
                = New Db4oEmbeddedReplicationProvider(desktopDatabase)
            Dim mobileReplicationProvider As IReplicationProvider _
                = New Db4oEmbeddedReplicationProvider(mobileDatabase)

            ' #example: Register a listener for information about the replication process
            Dim logReplicationListener As IReplicationEventListener = New LogReplicationListener()

            Dim replicationSession As IReplicationSession _
                = Replication.Begin(dektopReplicationProvider, mobileReplicationProvider, logReplicationListener)
            ' #end example
            ReplicateBidirectional(replicationSession)

            replicationSession.Commit()
            CloseDBs(desktopDatabase, mobileDatabase)
        End Sub

        Private Shared Sub ReplicationConflicts()
            DeleteDatabases()

            Dim desktopDatabase As IObjectContainer = OpenDatabase(DesktopDatabaseName)
            desktopDatabase.Store(New Pilot("Max"))
            Dim mobileDatabase As IObjectContainer = OpenDatabase(MobileDatabaseName)

            ReplicateBidirectional(desktopDatabase, mobileDatabase)


            UpdateObject(desktopDatabase)
            UpdateObject(mobileDatabase)

            Dim dektopReplicationProvider As IReplicationProvider _
                = New Db4oEmbeddedReplicationProvider(desktopDatabase)
            Dim mobileReplicationProvider As IReplicationProvider _
                = New Db4oEmbeddedReplicationProvider(mobileDatabase)

            ' #example: Deal with conflicts
            Dim replicationListener As IReplicationEventListener = New SimpleConflictResolvingListener()

            Dim replicationSession As IReplicationSession _
                = Replication.Begin(dektopReplicationProvider, mobileReplicationProvider, replicationListener)
            ' #end example

            ReplicateBidirectional(replicationSession)
            replicationSession.Commit()

            CloseDBs(desktopDatabase, mobileDatabase)
        End Sub

        Private Shared Sub ReplicationConflictTakeLatestChange()
            DeleteDatabases()

            Dim desktopDatabase As IObjectContainer = OpenDatabase(DesktopDatabaseName)
            desktopDatabase.Store(New Pilot("Max"))
            Dim mobileDatabase As IObjectContainer = OpenDatabase(MobileDatabaseName)

            ReplicateBidirectional(desktopDatabase, mobileDatabase)


            UpdateObject(desktopDatabase)
            UpdateObject(mobileDatabase)

            Dim dektopReplicationProvider As IReplicationProvider _
                = New Db4oEmbeddedReplicationProvider(desktopDatabase)
            Dim mobileReplicationProvider As IReplicationProvider _
                = New Db4oEmbeddedReplicationProvider(mobileDatabase)

            ' #example: Take latest change
            Dim replicationListener As IReplicationEventListener = _
                New TakeLatestModificationOnConflictListener()

            Dim replicationSession As IReplicationSession _
                = Replication.Begin(dektopReplicationProvider, mobileReplicationProvider, replicationListener)
            ' #end example

            ReplicateBidirectional(replicationSession)
            replicationSession.Commit()

            CloseDBs(desktopDatabase, mobileDatabase)
        End Sub


        Private Shared Sub ConcurrencyLimitations()
            DeleteDatabases()

            ' #example: Lost replication
            Dim serverDatabase As IObjectServer = OpenDatabaseServer(DesktopDatabaseName)
            Dim mobileDatabase As IObjectContainer = OpenDatabase(MobileDatabaseName)

            If True Then
                Dim serverDbConnection As IObjectContainer = _
                    Db4oClientServer.OpenClient(Host, Port, UserName, UserName)
                serverDbConnection.Store(New Pilot("Pilot 1"))
                serverDbConnection.Commit()

                ' The replication starts here
                Dim connectionForReplication As IObjectContainer = _
                    Db4oClientServer.OpenClient(Host, Port, UserName, UserName)
                Dim replicationSession As IReplicationSession = _
                    Replication.Begin(connectionForReplication, mobileDatabase)
                Dim changesOnDesktop As IObjectSet = _
                    replicationSession.ProviderA().ObjectsChangedSinceLastReplication()

                ' during the replication other clients store data on the server
                serverDbConnection.Store(New Pilot("Pilot 2"))
                serverDbConnection.Commit()

                For Each changedObjectOnDesktop As Object In changesOnDesktop
                    replicationSession.Replicate(changedObjectOnDesktop)
                Next

                replicationSession.Commit()

                serverDbConnection.Store(New Pilot("Pilot 3"))
                serverDbConnection.Commit()
            End If

            ' Pilot 2 is not replicated
            PrintPilots(mobileDatabase)


            If True Then
                Dim connectionForReplication As IObjectContainer = _
                    Db4oClientServer.OpenClient(Host, Port, UserName, UserName)
                Dim replicationSession As IReplicationSession = _
                    Replication.Begin(connectionForReplication, mobileDatabase)
                Dim changesOnDesktop As IObjectSet = _
                    replicationSession.ProviderA().ObjectsChangedSinceLastReplication()

                For Each changedOnDesktop As Object In changesOnDesktop
                    replicationSession.Replicate(changedOnDesktop)
                Next
                replicationSession.Commit()
            End If

            ' Pilot 2 is still not replicated
            PrintPilots(mobileDatabase)
            ' #end example

            serverDatabase.Close()
            mobileDatabase.Close()
        End Sub


        Private Shared Sub SimpleMigration()
            DeleteDatabases()

            Dim desktopDatabaseWithoutUUID As IObjectContainer = Db4oEmbedded.OpenFile(DesktopDatabaseName)
            desktopDatabaseWithoutUUID.Store(New Pilot("Max"))
            desktopDatabaseWithoutUUID.Store(New Pilot("Joe"))
            desktopDatabaseWithoutUUID.Commit()
            desktopDatabaseWithoutUUID.Close()

            Dim desktopDatabase As IObjectContainer = OpenDatabase(DesktopDatabaseName)
            Dim mobileDatabase As IObjectContainer = OpenDatabase(MobileDatabaseName)

            ' #example: Updating all objects ensures that it has a UUID and timestamp
            Dim allObjects As IList(Of Object) = desktopDatabase.Query(Of Object)()
            For Each objectToUpdate As Object In allObjects
                desktopDatabase.Store(objectToUpdate)
            Next
            desktopDatabase.Commit()
            ' #end example

            ReplicateBidirectional(desktopDatabase, mobileDatabase)

            PrintPilots(mobileDatabase)

            CloseDBs(desktopDatabase, mobileDatabase)
        End Sub


        Private Shared Sub MigrationOnTheFly()
            DeleteDatabases()

            Dim desktopDatabaseWithoutUUID As IObjectContainer = Db4oEmbedded.OpenFile(DesktopDatabaseName)
            desktopDatabaseWithoutUUID.Store(New Car(New Pilot("Max"), "Max's Car"))
            desktopDatabaseWithoutUUID.Store(New Car(New Pilot("Joe"), "Joe's Car"))
            desktopDatabaseWithoutUUID.Commit()
            desktopDatabaseWithoutUUID.Close()

            Dim desktopDatabase As IObjectContainer = OpenDatabase(DesktopDatabaseName)
            Dim mobileDatabase As IObjectContainer = OpenDatabase(MobileDatabaseName)

            ' #example: Migrate on the fly
            Dim replicationSession As IReplicationSession = _
                Replication.Begin(desktopDatabase, mobileDatabase)
            Dim initialReplication As IList(Of Car) = desktopDatabase.Query(Of Car)()

            For Each changedObjectOnDesktop As Car In initialReplication
                Dim infoAboutObject As IObjectInfo = desktopDatabase.Ext() _
                    .GetObjectInfo(changedObjectOnDesktop)
                If infoAboutObject.GetUUID() Is Nothing Then
                    desktopDatabase.Ext().Store(changedObjectOnDesktop, 2)
                End If
                replicationSession.Replicate(changedObjectOnDesktop)
            Next
            replicationSession.Commit()
            ' #end example

            PrintCars(mobileDatabase)

            CloseDBs(desktopDatabase, mobileDatabase)
        End Sub


        Private Shared Sub UpdateObject(ByVal desktopDatabase As IObjectContainer)
            Dim pilotOnDesktop As Pilot = desktopDatabase.Query(Of Pilot)()(0)
            pilotOnDesktop.Points = 200
            desktopDatabase.Store(pilotOnDesktop)
            desktopDatabase.Commit()
        End Sub


        Private Shared Sub ReplicateBidirectional(ByVal desktopDatabase As IObjectContainer, ByVal mobileDatabase As IObjectContainer)
            Dim replicationSession As IReplicationSession = Replication.Begin(desktopDatabase, mobileDatabase)
            ReplicateBidirectional(replicationSession)
            replicationSession.Commit()
        End Sub


        Private Shared Sub ReplicateBidirectional(ByVal replication As IReplicationSession)
            Dim changesOnDesktop As IObjectSet = replication.ProviderA().ObjectsChangedSinceLastReplication()
            Dim changesOnMobile As IObjectSet = replication.ProviderB().ObjectsChangedSinceLastReplication()

            For Each changedObjectOnDesktop As Object In changesOnDesktop
                replication.Replicate(changedObjectOnDesktop)
            Next

            For Each changedObjectOnMobile As Object In changesOnMobile
                replication.Replicate(changedObjectOnMobile)
            Next
        End Sub

        Private Shared Function OpenDatabase(ByVal fileName As String) As IObjectContainer
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.File.GenerateUUIDs = ConfigScope.Globally
            configuration.File.GenerateCommitTimestamps = True
            Return Db4oEmbedded.OpenFile(configuration, fileName)
        End Function

        Private Shared Function OpenDatabaseServer(ByVal fileName As String) As IObjectServer
            Dim configuration As IServerConfiguration = Db4oClientServer.NewServerConfiguration()
            configuration.File.GenerateUUIDs = ConfigScope.Globally
            configuration.File.GenerateCommitTimestamps = True
            Dim srv As IObjectServer = Db4oClientServer.OpenServer(configuration, fileName, Port)
            srv.GrantAccess(UserName, UserName)
            Return srv
        End Function

        Private Shared Sub StoreObjectsIn(ByVal databaseFile As String)
            Dim db As IObjectContainer = OpenDatabase(databaseFile)
            Dim john As New Pilot("John", 100)
            Dim johnsCar As New Car(john, "John's Car")
            db.Store(johnsCar)
            Dim max As New Pilot("Max", 200)
            Dim maxsCar As New Car(max, "Max's Car")
            db.Store(maxsCar)
            db.Commit()
            db.Close()
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


        Private Shared Sub CloseDBs(ByVal ParamArray databases As IObjectContainer())
            For Each db As IObjectContainer In databases
                db.Dispose()
            Next
        End Sub


        Private Shared Sub DeleteDatabases()
            File.Delete(DesktopDatabaseName)
            File.Delete(MobileDatabaseName)
        End Sub


        ' #example: The ReplicationEventListener for informations about the replication process
        Private Class LogReplicationListener
            Implements IReplicationEventListener
            Public Sub OnReplicate(ByVal replicationEvent As IReplicationEvent) _
                                            Implements IReplicationEventListener.OnReplicate
                Dim stateInDesktop As IObjectState = replicationEvent.StateInProviderA()
                If stateInDesktop.IsNew() Then
                    Console.WriteLine("Object '{0}' is new on desktop database", _
                                      stateInDesktop.GetObject())
                End If
                If stateInDesktop.WasModified() Then
                    Console.WriteLine("Object '{0}' was modified on desktop database", _
                                      stateInDesktop.GetObject())
                End If
            End Sub
        End Class
        ' #end example


        ' #example: Conflict resolving listener
        Private Class SimpleConflictResolvingListener
            Implements IReplicationEventListener
            Public Sub OnReplicate(ByVal replicationEvent As IReplicationEvent) _
                                             Implements IReplicationEventListener.OnReplicate
                If replicationEvent.IsConflict() Then
                    Dim stateOfTheDesktop As IObjectState = replicationEvent.StateInProviderA()
                    replicationEvent.OverrideWith(stateOfTheDesktop)
                End If
            End Sub
        End Class
        ' #end example


        ' #example: Listener which takes latest changes
        Private Class TakeLatestModificationOnConflictListener
            Implements IReplicationEventListener
            Public Sub OnReplicate(ByVal replicationEvent As IReplicationEvent) _
                                    Implements IReplicationEventListener.OnReplicate
                If replicationEvent.IsConflict() Then
                    Dim stateOfTheDesktop As IObjectState = replicationEvent.StateInProviderA()
                    Dim stateOfTheMobile As IObjectState = replicationEvent.StateInProviderB()

                    If stateOfTheDesktop.ModificationDate() >= stateOfTheMobile.ModificationDate() Then
                        replicationEvent.OverrideWith(stateOfTheDesktop)
                    Else
                        replicationEvent.OverrideWith(stateOfTheMobile)
                    End If
                End If
            End Sub
        End Class
        ' #end example
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
