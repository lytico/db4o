Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Defragment
Imports Db4objects.Db4o.IO

Namespace Db4oDoc.Code.Deframentation
    Public Class DefragmentationConfigurationExamples
        Public Sub ConfigureFile()
            ' #example: Configure the file
            Dim config As New DefragmentConfig("database.db4o")

            Defragment.Defrag(config)
            ' #end example
        End Sub

        Public Sub ConfigureBackupFile()
            ' #example: Configure the file and backup file
            Dim config As New DefragmentConfig("database.db4o", "database.db4o.back")

            Defragment.Defrag(config)
            ' #end example
        End Sub

        Public Sub SetMappingImplementation()
            ' #example: Choose a id mapping system
            Dim mapping As IIdMapping = New InMemoryIdMapping()
            Dim config As New DefragmentConfig("database.db4o", "database.db4o.back", mapping)

            Defragment.Defrag(config)
            ' #end example
        End Sub

        Public Sub SetDb4OConfiguration()
            ' #example: Use the database-configuration
            Dim config As New DefragmentConfig("database.db4o")
            ' It's best to use the very same configuration you use for the regular database
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            config.Db4oConfig(configuration)

            Defragment.Defrag(config)
            ' #end example
        End Sub

        Public Sub SetCommitFrequency()
            ' #example: Set the commit frequency
            Dim config As New DefragmentConfig("database.db4o")
            config.ObjectCommitFrequency(10000)

            Defragment.Defrag(config)
            ' #end example
        End Sub

        Public Sub ChangeBackupStorage()
            ' #example: Use a separate storage for the backup
            Dim config As New DefragmentConfig("database.db4o")
            config.BackupStorage(New FileStorage())

            Defragment.Defrag(config)
            ' #end example
        End Sub

        Public Sub DeleteBackup()
            ' #example: Delete the backup after the defragmentation process
            Dim config As New DefragmentConfig("database.db4o")
            config.ForceBackupDelete(True)

            Defragment.Defrag(config)
            ' #end example
        End Sub

        Public Sub DisableReadOnlyForBackup()
            ' #example: Disable readonly on backup
            Dim config As New DefragmentConfig("database.db4o")
            config.ReadOnly(False)

            Defragment.Defrag(config)
            ' #end example
        End Sub

        Public Sub UseAClassFilter()
            ' #example: Use class filter
            Dim config As New DefragmentConfig("database.db4o")
            config.StoredClassFilter(New AvailableTypeFilter())

            Defragment.Defrag(config)
            ' #end example
        End Sub

        Public Sub UpgradeDb4OFile()
            ' #example: Upgrade database version
            Dim config As New DefragmentConfig("database.db4o")
            config.UpgradeFile(Environment.GetEnvironmentVariable("TEMP"))

            Defragment.Defrag(config)
            ' #end example
        End Sub
    End Class
End Namespace