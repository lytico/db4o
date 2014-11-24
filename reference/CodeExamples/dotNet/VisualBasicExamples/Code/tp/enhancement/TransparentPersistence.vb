Imports System.Linq
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Linq
Imports Db4objects.Db4o.TA

Namespace Db4oDoc.Code.Tp.Enhancement
    Public Class TransparentPersistence
        Private Const DatabaseFileName As String = "database.db4o"

        Public Shared Sub Main(args As String())
            CheckEnhancement()
            StoreExampleObjects()
            ActivationJustWorks()
            UpdatingJustWorks()
        End Sub

        Private Shared Sub CheckEnhancement()
            ' #example: Check for enhancement
            If Not GetType(IActivatable).IsAssignableFrom(GetType(Person)) Then
                Throw New InvalidOperationException(String.Format("Expect that the {0} implements {1}", GetType(Person), GetType(IActivatable)))
            End If
            ' #end example
        End Sub

        Private Shared Sub ActivationJustWorks()
            Using container As IObjectContainer = OpenDatabaseWithTA()
                ' #example: Activation just works
                Dim person As Person = QueryByName(container, "Joanna the 10")
                Dim beginOfDynasty As Person = person.Mother

                ' With transparent activation enabled, you can navigate deeply
                ' nested object graphs. db4o will ensure that the objects
                ' are loaded from the database.
                While beginOfDynasty.Mother IsNot Nothing
                    beginOfDynasty = beginOfDynasty.Mother
                End While
                Console.WriteLine(beginOfDynasty.Name)
                ' #end example
            End Using
        End Sub

        Private Shared Sub UpdatingJustWorks()
            Using container As IObjectContainer = OpenDatabaseWithTP()
                ' #example: Just update and commit. Transparent persistence manages all updates
                Dim person As Person = QueryByName(container, "Joanna the 10")
                Dim mother As Person = person.Mother
                mother.Name = "New Name"
                ' Just commit the transaction. All modified objects are stored
                container.Commit()
                ' #end example
            End Using
        End Sub

        Private Shared Function OpenDatabaseWithTA() As IObjectContainer
            ' #example: Add transparent activation
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.Add(New TransparentActivationSupport())
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFileName)
            ' #end example
            Return container
        End Function
        Private Shared Function OpenDatabaseWithTP() As IObjectContainer
            ' #example: Add transparent persistence
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.Add(New TransparentPersistenceSupport(New DeactivatingRollbackStrategy()))
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFileName)
            ' #end example
            Return container
        End Function

        Private Shared Function QueryByName(container As IObjectContainer, nameOfPerson As String) As Person
            Return (From p As Person In container _
                Where p.Name = nameOfPerson _
                Select p).First()
        End Function

        Private Shared Sub StoreExampleObjects()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFileName)
                Dim pers = Person.PersonWithHistory()
                container.Store(pers)
            End Using
        End Sub
    End Class
End Namespace
