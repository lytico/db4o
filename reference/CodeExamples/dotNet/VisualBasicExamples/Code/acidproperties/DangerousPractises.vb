Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.IO

Namespace Db4oDoc.Code.AcidProperties
    Public Class DangerousPractises
        Public Shared Sub DangerousStorage()
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            ' #example: Using the non-flushing storage weakens the ACID-properties
            Dim fileStorage As IStorage = New FileStorage()
            configuration.File.Storage = New NonFlushingStorage(fileStorage)
            ' #end example
            Db4oEmbedded.OpenFile(configuration, "database.db4o")
        End Sub
        Public Shared Sub DangerousNonRecovering()
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            ' #example: Disabling commit-recovery weakens the ACID-properties
            configuration.File.DisableCommitRecovery()
            ' #end example
            Db4oEmbedded.OpenFile(configuration, "database.db4o")
        End Sub
    End Class
End Namespace
