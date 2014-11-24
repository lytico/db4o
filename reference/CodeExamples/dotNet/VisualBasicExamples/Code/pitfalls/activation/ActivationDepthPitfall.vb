Imports System.IO
Imports System.Linq
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Linq

Namespace Db4oDoc.Code.Pitfalls.Activation
    Public Class ActivationDepthPitfall
        Public Const DatabaseFile As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            CleanUp()
            PrepareDeepObjGraph()

            Try
                RunIntoActivationIssue()
            Catch e As Exception
                Console.WriteLine(e.StackTrace)
            End Try

            FixItWithHigherActivationDepth()
            FixItWithExplicitlyActivating()
            Deactivate()
        End Sub

        Private Shared Sub FixItWithHigherActivationDepth()
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ActivationDepth = 16
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
                Dim jodie As Person = QueryForJodie(container)

                Dim julia As Person = jodie.Mother.Mother.Mother.Mother.Mother

                Console.WriteLine(julia.Name)
                Dim joannaName As String = julia.Mother.Name
                Console.WriteLine(joannaName)
            End Using
        End Sub
        Private Shared Sub FixItWithExplicitlyActivating()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("database.db4o")
                Dim jodie As Person = QueryForJodie(container)

                ' #example: Fix with explicit activation
                Dim julia As Person = jodie.Mother.Mother.Mother.Mother.Mother
                container.Activate(julia, 5)

                Console.WriteLine(julia.Name)
                Dim joannaName As String = julia.Mother.Name
                ' #end example
                Console.WriteLine(joannaName)
            End Using
        End Sub

        Private Shared Sub Deactivate()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("database.db4o")
                Dim jodie = QueryForJodie(container)

                container.Activate(jodie, 5)

                ' #example: Deactivate an object
                Console.WriteLine(jodie.Name)
                container.Deactivate(jodie, 5)
                ' Now all fields will be null or 0
                ' The same applies for all references objects up to a depth of 5 
                Console.WriteLine(jodie.Name)
                ' #end example
            End Using
        End Sub

        Private Shared Sub RunIntoActivationIssue()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: Run into not activated objects
                Dim jodie As Person = QueryForJodie(container)

                Dim julia As Person = jodie.Mother.Mother.Mother.Mother.Mother

                ' This will print null
                ' Because julia is not activated
                ' and therefore all fields are not set
                Console.WriteLine(julia.Name)
                ' This will throw a NullPointerException.
                ' Because julia is not activated
                ' and therefore all fields are not set
                Dim joannaName As String = julia.Mother.Name
                ' #end example
            End Using
        End Sub

        Private Shared Sub CleanUp()
            File.Delete(DatabaseFile)
        End Sub

        Private Shared Function QueryForJodie(ByVal container As IObjectContainer) As Person
            Return (From p As Person In container Where p.Name = "Jodie").First()
        End Function

        Private Shared Sub PrepareDeepObjGraph()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                Dim joanna As New Person("Joanna")
                Dim jenny As New Person(joanna, "Jenny")
                Dim julia As New Person(jenny, "Julia")
                Dim jill As New Person(julia, "Jill")
                Dim joel As New Person(jill, "Joel")
                Dim jamie As New Person(joel, "Jamie")
                Dim jodie As New Person(jamie, "Jodie")
                container.Store(jodie)
            End Using
        End Sub
    End Class


End Namespace
