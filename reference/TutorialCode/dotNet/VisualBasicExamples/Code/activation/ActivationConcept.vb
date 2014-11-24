Imports System.Linq
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Linq

Namespace Db4oTutorialCode.Code.Activation
    Public Class ActivationConcept
        Private Const DatabaseFile As String = "database.db4o"

        Public Shared Sub Main(args As String())
            StoreExampleObjects()
            RunningIntoActivationLimit()
            DealWithActivation()
            IncreaseActivationDepth()
        End Sub

        Private Shared Sub StoreExampleObjects()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: Store a deep object hierarchy
                Dim eva = New Person("Eva", Nothing)
                Dim julia = New Person("Julia", eva)
                Dim jennifer = New Person("Jennifer", julia)
                Dim jamie = New Person("Jamie", jennifer)
                Dim jill = New Person("Jill", jamie)
                Dim joanna = New Person("Joanna", jill)

                Dim joelle = New Person("Joelle", joanna)
                ' #end example
                container.Store(joelle)
            End Using
        End Sub

        Private Shared Sub RunningIntoActivationLimit()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: Activation depth in action
                Dim joelle As Person = QueryForJoelle(container)
                Dim jennifer As Person = joelle.Mother.Mother.Mother.Mother
                Console.WriteLine("Is activated: {0}", jennifer)
                ' Now we step across the activation boundary
                ' therefore the next person isn't activate anymore.
                ' That means all fields are set to null or default-value
                Dim julia As Person = jennifer.Mother
                Console.WriteLine("Isn't activated anymore {0}", julia)
                ' #end example

                Try
                    ' #example: NullPointer exception due to not activated objects
                    Dim nameOfMother As String = julia.Mother.Name
                    ' #end example
                Catch ex As NullReferenceException
                    Console.WriteLine("Exception due to not activated object {0}", ex)
                End Try
            End Using
        End Sub

        Private Shared Sub DealWithActivation()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                Dim joelle As Person = QueryForJoelle(container)
                Dim julia As Person = joelle.Mother.Mother.Mother.Mother.Mother

                ' #example: Check if an instance is activated
                Dim isActivated As Boolean = container.Ext().IsActive(julia)
                ' #end example
                Console.WriteLine("Is activated? {0}", isActivated)
                ' #example: Activate instance to a depth of five
                container.Activate(julia, 5)
                ' #end example
                Console.WriteLine("Is activated? {0}", container.Ext().IsActive(julia))
            End Using
        End Sub

        Private Shared Sub IncreaseActivationDepth()
            ' #example: Increase the activation depth to 10
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ActivationDepth = 10
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
                ' #end example
                Dim joelle As Person = QueryForJoelle(container)
                Dim julia As Person = joelle.Mother.Mother.Mother.Mother.Mother

                Dim isActivated As Boolean = container.Ext().IsActive(julia)
                Console.WriteLine("Is activated? {0}", isActivated)
            End Using
        End Sub

        Private Shared Sub MoreActivationOptions()
            ' #example: More activation options
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            ' At least activate persons to a depth of 10
            configuration.Common.ObjectClass(GetType(Person)).MinimumActivationDepth(10)
            ' Or maybe we just want to activate all referenced objects
            configuration.Common.ObjectClass(GetType(Person)).CascadeOnActivate(True)
            ' #end example
        End Sub

        Private Shared Function QueryForJoelle(container As IObjectContainer) As Person
            Return (From p As Person In container _
                    Where p.Name = "Joelle" _
                    Select p).Single()
        End Function
    End Class
End Namespace
