
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config

Namespace Db4oDoc.Code.Configuration.Objectfield
    Public Class ObjectFieldConfigurations
        Private Const DatabaseFile As String = "database.db4o"

        Private Shared Sub IndexField()
            ' #example: Index a certain field
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(Person)).ObjectField("name").Indexed(True)
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub CascadeOnActivate()
            ' #example: When activated, activate also the object referenced by this field
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(Person)).ObjectField("father").CascadeOnActivate(True)
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub CascadeOnUpdate()
            ' #example: When updated, update also the object referenced by this field
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(Person)).ObjectField("father").CascadeOnUpdate(True)
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub CascadeOnDelete()
            ' #example: When deleted, delete also the object referenced by this field
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(Person)).ObjectField("father").CascadeOnDelete(True)
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub RenameField()
            ' #example: Rename this field
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(Person)).ObjectField("name").Rename("sirname")
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub
    End Class

    Public Class Person
        Private name As String
        Private father As Person
    End Class
End Namespace