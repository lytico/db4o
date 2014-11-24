Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.IO

Namespace Db4oDoc.Silverlight.Configuration.IO
    Public Class IOExamples
        Public Sub AddSilverlightSupport()
            ' #example: Add Silverlight support
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.AddConfigurationItem(New SilverlightSupport())

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            ' #end example
        End Sub
        Public Sub UseIsolatedStorage()
            ' #example: use the isolated storage on silverlight
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.File.Storage = New IsolatedStorageStorage()

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            ' #end example
        End Sub
    End Class
End Namespace