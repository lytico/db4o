Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config

Namespace Db4oDoc.Code.Configuration.File
    Public Class FileConfiguration
        Public Shared Sub AsynchronousSync()
            ' #example: Allow asynchronous synchronisation of the file-flushes
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.File.AsynchronousSync = True
            ' #end example
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            container.Close()

        End Sub

        Public Shared Sub ChangeBlobPath()
            ' #example: Configure the blob-path
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.File.BlobPath = "myBlobDirectory"
            ' #end example
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            container.Close()

        End Sub

        Public Shared Sub IncreaseBlockSize()
            ' #example: Increase block size for larger databases
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.File.BlockSize = 8
            ' #end example
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            container.Close()
        End Sub

        Public Shared Sub GlobalUUID()
            ' #example: Enable db4o uuids globally
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.File.GenerateUUIDs = ConfigScope.Globally
            ' #end example
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            container.Close()
        End Sub

        Public Shared Sub IndividualUUID()
            ' #example: Enable db4o uuids for certain classes
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.File.GenerateUUIDs = ConfigScope.Individually
            configuration.Common.ObjectClass(GetType(SpecialClass)).GenerateUUIDs(True)
            ' #end example
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")

            Dim withUUID As New SpecialClass()
            container.Store(withUUID)
            Dim withoutUUID As New NormalClass()
            container.Store(withoutUUID)

            AssertNotNull(container.Ext().GetObjectInfo(withUUID).GetUUID())
            AssertNull(container.Ext().GetObjectInfo(withoutUUID).GetUUID())

            container.Close()
        End Sub

        Public Shared Sub CommitTimestamps()
            ' #example: Enable db4o commit timestamps
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.File.GenerateCommitTimestamps = True
            ' #end example
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            container.Close()
        End Sub

        Public Shared Sub ReserveSpace()
            ' #example: Configure the growth size
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.File.DatabaseGrowthSize = 4096
            ' #end example
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            container.Close()

        End Sub
        Public Shared Sub DisableCommitRecovers()
            ' #example: Disable commit recovery
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.File.DisableCommitRecovery()
            ' #end example
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            container.Close()

        End Sub

        Public Shared Sub DoNotLockDatabaseFile()
            ' #example: Disable the database file lock
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.File.LockDatabaseFile = False
            ' #end example
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            container.Close()
        End Sub

        Public Shared Sub ReadOnlyMode()
            ' #example: Set read only mode
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.File.ReadOnly = True
            ' #end example
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            container.Close()

        End Sub
        Public Shared Sub RecoveryMode()
            ' #example: Enable recovery mode to open a corrupted database
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.File.RecoveryMode = True
            ' #end example
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            container.Close()
        End Sub
        Public Shared Sub ReserveStorageSpace()
            ' #example: Reserve storage space
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.File.ReserveStorageSpace = 1024 * 1024
            ' #end example
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            container.Close()
        End Sub

        Private Shared Sub AssertNotNull(ByVal uuid As Object)
            If uuid Is Nothing Then
                Throw New Exception("Expected not null")
            End If
        End Sub
        Private Shared Sub AssertNull(ByVal uuid As Object)
            If uuid IsNot Nothing Then
                Throw New Exception("Expected null")
            End If
        End Sub
        Private Shared Sub AssertTrue(ByVal value As Boolean)
            If value Then
                Throw New Exception("Expected true")
            End If
        End Sub

    End Class

    Class SpecialClass

    End Class
    Class NormalClass

    End Class

End Namespace