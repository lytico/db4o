Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config

Namespace Db4oDoc.Code.Configuration.ObjectConfig
    Public Class ObjectConfigurationExamples
        Private Const DatabaseFile As String = "database.db4o"


        Private Shared Sub CallConstructor()
            ' #example: Call constructor
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(Person)).CallConstructor(True)
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub CascadeOnActivate()
            ' #example: When activated, activate also all references
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(Person)).CascadeOnActivate(True)
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub CascadeOnDelete()
            ' #example: When deleted, delete also all references
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(Person)).CascadeOnDelete(True)
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub CascadeOnUpdate()
            ' #example: When updated, update also all references
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(Person)).CascadeOnUpdate(True)
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub GenerateUuiDs()
            ' #example: Generate uuids for this type
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(Person)).GenerateUUIDs(True)
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub IndexObjects()
            ' #example: Disable class index
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(Person)).Indexed(False)
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub MaximumActivationDepth()
            ' #example: Set maximum activation depth
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(Person)).MaximumActivationDepth(5)
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub MinimalActivationDepth()
            ' #example: Set minimum activation depth
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(Person)).MinimumActivationDepth(2)
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub PersistStaticFieldValues()
            ' #example: Persist also the static fields
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(Person)).PersistStaticFieldValues()
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub Rename()
            ' #example: Rename this type
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(Person)).Rename("Db4oDoc.NewNamespace.NewName")
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub StoreTransientFields()
            ' #example: Store also transient fields
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(Person)).StoreTransientFields(True)
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub

        Private Shared Sub Translator()
            ' #example: Use a translator for this type
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(Person)).Translate(New TSerializable())
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub


        Private Shared Sub UpdateDepth()
            ' #example: Set the update depth
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(Person)).UpdateDepth(2)
            ' #end example

            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
            container.Close()
        End Sub
    End Class


    Public Class Person
    End Class
End Namespace