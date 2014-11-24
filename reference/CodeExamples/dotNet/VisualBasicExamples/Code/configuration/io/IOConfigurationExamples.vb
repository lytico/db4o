Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.IO

Namespace Db4oDoc.Code.Configuration.IO
    Public Class IOConfigurationExamples

        Public Shared Sub FileStorage()
            ' #example: Using the pure file storage
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            Dim fileStorage As IStorage = New FileStorage()
            configuration.File.Storage = fileStorage
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            ' #end example

        End Sub
        Public Shared Sub CachingStorage()
            ' #example: Using a caching storage
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            Dim fileStorage As IStorage = New FileStorage()
            Dim cachingStorage As IStorage = New CachingStorage(fileStorage, 128, 1024)
            configuration.File.Storage = cachingStorage
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            ' #end example

        End Sub

        Public Shared Sub NonFlushingStorage()
            ' #example: Using the non-flushing storage
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            Dim fileStorage As IStorage = New FileStorage()
            ' the non-flushing storage improves performance, but risks database corruption.
            Dim cachingStorage As IStorage = New NonFlushingStorage(FileStorage)
            configuration.File.Storage = cachingStorage
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(Configuration, "database.db4o")
            ' #end example
        End Sub



        Public Shared Sub SpecifyGrowStrategyForMemoryStorage()
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            ' #example: Using memory-storage with constant grow strategy
            Dim growStrategy As IGrowthStrategy = New ConstantGrowthStrategy(100)
            Dim memory As New MemoryStorage(growStrategy)
            configuration.File.Storage = memory
            ' #end example
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
        End Sub

        Public Shared Sub UsingMemoryStorage()
            ' #example: Using memory-storage
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            Dim memory As New MemoryStorage()
            configuration.File.Storage = memory
            ' #end example
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
        End Sub

        Public Shared Sub UsingPagingMemoryStorage()
            ' #example: Using paging memory-storage
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            Dim memory As New PagingMemoryStorage()
            configuration.File.Storage = memory
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")

            ' #end example
            container.Close()

        End Sub

        Public Shared Sub StorageStack()
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            ' #example: You stack up different storage-decorator to add functionality
            ' the basic file storage
            Dim fileStorage As IStorage = New FileStorage()
            ' add your own decorator
            Dim myStorageDecorator As IStorage = New MyStorageDecorator(fileStorage)
            ' add caching to the storage
            Dim storageWithCaching As IStorage = New CachingStorage(myStorageDecorator)
            ' finally configure db4o with our storage-stack
            configuration.File.Storage = storageWithCaching
            ' #end example
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
        End Sub
    End Class

    '
    'This decorator does nothing. It's just used as an example
    '   
    Friend Class MyStorageDecorator
        Inherits StorageDecorator
        Public Sub New(ByVal storage As IStorage)
            MyBase.New(storage)
        End Sub
    End Class
End Namespace
