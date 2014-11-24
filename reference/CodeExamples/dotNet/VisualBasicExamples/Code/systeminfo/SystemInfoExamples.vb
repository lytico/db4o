Imports Db4objects.Db4o

Namespace Db4oDoc.Code.SystemInfo
    Public Class SystemInfoExamples
        Public Shared Sub Main(ByVal args As String())
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("database.db4o")
                ' #example: Freespace size info
                Dim freeSpaceSize As Long = container.Ext().SystemInfo().FreespaceSize()
                Console.WriteLine("Freespace in bytes: {0}", freeSpaceSize)
                ' #end example

                ' #example: Freespace entry count info
                Dim freeSpaceEntries As Integer = container.Ext().SystemInfo().FreespaceEntryCount()
                Console.WriteLine("Freespace-entries count: {0}", freeSpaceEntries)
                ' #end example

                ' #example: Database size info
                Dim databaseSize As Long = container.Ext().SystemInfo().TotalSize()
                ' #end example
                Console.WriteLine("Database size: {0}", databaseSize)
            End Using
        End Sub
    End Class
End Namespace
