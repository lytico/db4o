Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.IO

Namespace Db4oDoc.Code.Configuration.FreeSpace
    Public Class FreeSpaceConfiguration
        Public Shared Sub DiscardSettings()
            ' #example: Discard settings
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            ' discard smaller than 256 bytes
            configuration.File.Freespace.DiscardSmallerThan(256)
            ' #end example
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            container.Dispose()

        End Sub
        Public Shared Sub UseBTreeSystem()
            ' #example: Use BTree system
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.File.Freespace.UseBTreeSystem()
            ' #end example
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            container.Dispose()
        End Sub
        Public Shared Sub UseInMemorySystem()
            ' #example: Use the in memory system
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.File.Freespace.UseRamSystem()
            ' #end example
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            container.Dispose()
        End Sub
        Public Shared Sub FreespaceFiller()
            ' #example: Using a freespace filler
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.File.Freespace.FreespaceFiller(New MyFreeSpaceFiller())
            ' #end example
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            container.Dispose()
        End Sub

    End Class


    ' #example: The freespace filler
    Class MyFreeSpaceFiller
        Implements IFreespaceFiller
        Public Sub Fill(ByVal block As BlockAwareBinWindow) Implements IFreespaceFiller.Fill
            Dim emptyBytes As Byte() = New Byte(block.Length() - 1) {}
            block.Write(0, emptyBytes)
        End Sub
    End Class
    ' #end example
End Namespace
