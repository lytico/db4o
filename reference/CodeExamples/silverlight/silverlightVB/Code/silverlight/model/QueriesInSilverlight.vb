Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.IO
Imports Db4objects.Db4o.Linq

Namespace Db4oDoc.Silverlight.Model
    Public Class QueriesInSilverlight
        Private Sub SodaQuery()
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.File.Storage = New IsolatedStorageStorage()

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            ' #example: Queries in Silverlight
            Dim persons = From p As Person In container _
                          Where p.FirstName.Contains("Roman") _
                          Select p
            For Each p As Person In persons
                ' do something with the person
            Next
            ' #end example


        End Sub
    End Class
End Namespace