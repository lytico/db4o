Imports System
Imports System.IO
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.TA
Imports Db4objects.Db4o.Linq

Namespace Db4oDoc.Ta.Example
    Public Class TransparentActivationExamples
        Private Const DatabaseFileName As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            TransparentActivationExample()
            TransparentPersistenceExample()
        End Sub

        Private Shared Sub TransparentActivationExample()
            CleanUp()

            ' #example: Transparent activation in action
            Using container As IObjectContainer = OpenDatabaseTA()
                Dim joanna As Person = Person.PersonWithHistory()
                container.Store(joanna)
            End Using
            Using container As IObjectContainer = OpenDatabaseTA()
                Dim joanna As Person = QueryByName(container, "Joanna the 10")
                Dim beginOfDynasty As Person = joanna.Mother

                ' With transparent activation enabled, you can navigate deeply
                ' nested object graphs. db4o will ensure that the objects
                ' are loaded from the database.
                While beginOfDynasty.Mother IsNot Nothing
                    beginOfDynasty = beginOfDynasty.Mother
                End While
                Console.WriteLine(beginOfDynasty.Name)

                ' Updating a object doesn't requires no store call.
                ' Just change the objects and the call commit.
                beginOfDynasty.Name = "New Name"
                container.Commit()
            End Using
            ' #end example

            CleanUp()
        End Sub


        Private Shared Sub TransparentPersistenceExample()
            CleanUp()

            ' #example: Transparent persistence in action
            Using container As IObjectContainer = OpenDatabaseTP()
                Dim joanna As Person = Person.PersonWithHistory()
                container.Store(joanna)
            End Using
            Using container As IObjectContainer = OpenDatabaseTP()
                Dim joanna As Person = QueryByName(container, "Joanna the 10")
                Dim beginOfDynasty As Person = joanna.Mother

                ' With transparent persistence enabled, you can navigate deeply
                ' nested object graphs. db4o will ensure that the objects
                ' are loaded from the database.
                While beginOfDynasty.Mother IsNot Nothing
                    beginOfDynasty = beginOfDynasty.Mother
                End While
                Console.WriteLine(beginOfDynasty.Name)

                ' Updating a object doesn't requires no store call.
                ' Just change the objects and the call commit.
                beginOfDynasty.Name = "New Name"
                container.Commit()
            End Using
            Using container As IObjectContainer = OpenDatabaseTP()
                Dim joanna As Person = QueryByName(container, "New Name")
                ' The changes are stored, due to transparent persistence
                Console.WriteLine(joanna.Name)
            End Using
            ' #end example

            CleanUp()
        End Sub

        Private Shared Sub CleanUp()
            File.Delete(DatabaseFileName)
        End Sub

        Private Shared Function QueryByName(ByVal container As IObjectContainer, ByVal name As String) As Person
            Return (From p As Person In container _
                Where p.Name.Equals(name) _
                Select p).First()
        End Function

        Private Shared Function OpenDatabaseTP() As IObjectContainer
            ' #example: Activate transparent persistence
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.Add(New TransparentPersistenceSupport())
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFileName)
            ' #end example
            Return container
        End Function
        Private Shared Function OpenDatabaseTA() As IObjectContainer
            ' #example: Activate transparent activation
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.Add(New TransparentActivationSupport())
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFileName)
            ' #end example
            Return container
        End Function
    End Class

End Namespace
