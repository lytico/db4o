Imports System.Collections
Imports System.Collections.Generic
Imports System.IO
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config

Namespace Db4oDoc.Code.Query.NativeQueries
    Public Class NativeQueryExamples
        Private Const DatabaseFile As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            CleanUp()
            Dim cfg As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(cfg, DatabaseFile)
                StoreData(container)

                Equality(container)
                Comparison(container)
                RageOfValues(container)
                CombineComparisons(container)
                FollowReferences(container)
                QueryInSeparateClass(container)
                AnyCode(container)
            End Using
        End Sub

        Private Shared Sub Equality(ByVal container As IObjectContainer)
            ' #example: Check for equality of the name
            Dim result As IList(Of Pilot) = container.Query(Of Pilot)(AddressOf QueryJohns)
            ' #end example

            ListResult(result)
        End Sub

        ' #example: Query for John
        Private Shared Function QueryJohns(ByVal pilot As Pilot)
            Return pilot.Name = "John"
        End Function
        ' #end example


        Private Shared Sub Comparison(ByVal container As IObjectContainer)
            ' #example: Compare values to each other
            Dim result As IList(Of Pilot) = container.Query(Of Pilot)(AddressOf QueryAdults)
            ' #end example

            ListResult(result)
        End Sub

        ' #example: Query for adults
        Private Shared Function QueryAdults(ByVal pilot As Pilot)
            Return pilot.Age < 18
        End Function
        ' #end example

        Private Shared Sub RageOfValues(ByVal container As IObjectContainer)
            ' #example: Query for a particular rage of values
            Dim result As IList(Of Pilot) = container.Query(Of Pilot)(AddressOf QueryRange)
            ' #end example

            ListResult(result)
        End Sub

        ' #example: Query for range
        Private Shared Function QueryRange(ByVal pilot As Pilot)
            Return pilot.Age > 18 AndAlso pilot.Age < 30
        End Function
        ' #end example

        Private Shared Sub CombineComparisons(ByVal container As IObjectContainer)
            ' #example: Combine different comparisons with the logical operators
            Dim result As IList(Of Pilot) = container.Query(Of Pilot)(AddressOf CombineCriterias)
            ' #end example

            ListResult(result)
        End Sub

        ' #example: Combine criterias
        Private Shared Function CombineCriterias(ByVal pilot As Pilot)
            Return (pilot.Age > 18 AndAlso pilot.Age < 30) OrElse pilot.Name = "John"
        End Function
        ' #end example


        Private Shared Sub FollowReferences(ByVal container As IObjectContainer)
            ' #example: You can follow references
            Dim result As IList(Of Car) = container.Query(Of Car)(AddressOf QueryOnReferences)
            ' #end example

            ListResult(result)
        End Sub

        ' #example: Query on reference
        Private Shared Function QueryOnReferences(ByVal car As Car)
            Return car.Pilot.Name = "John"
        End Function
        ' #end example

        Private Shared Sub QueryInSeparateClass(ByVal container As IObjectContainer)
            ' #example: Use the predefined query
            Dim result As IList(Of Pilot) = container.Query(Of Pilot)(AddressOf AllJohns)
            ' #end example

            ListResult(result)
        End Sub

        ' #example: Query as method
        Private Shared Function AllJohns(ByVal pilot As Pilot) As Boolean
            Return pilot.Name = "John"
        End Function

        ' #end example


        Private Shared Sub AnyCode(ByVal container As IObjectContainer)
            ' #example: Arbitrary code
            Dim result As IList(Of Pilot) = container.Query(Of Pilot)(AddressOf QueryWithAnyCode)
            ' #end example

            ListResult(result)
        End Sub

        ' #example: Query with arbitrary code
        Private Shared Function QueryWithAnyCode(ByVal pilot As Pilot)
            Dim allowedAges As IList(Of Integer) = Array.AsReadOnly(New Integer() {18, 20, 35})
            Return allowedAges.Contains(Pilot.Age) _
                        OrElse Pilot.Name.ToLowerInvariant() = "John"
        End Function
        ' #end example


        Private Shared Sub CleanUp()
            File.Delete(DatabaseFile)
        End Sub


        Private Shared Sub ListResult(ByVal result As IEnumerable)
            For Each obj As Object In result
                Console.WriteLine(obj)
            Next
        End Sub

        Private Shared Sub StoreData(ByVal container As IObjectContainer)
            Dim john As New Pilot("John", 42)
            Dim joanna As New Pilot("Joanna", 45)
            Dim jenny As New Pilot("Jenny", 21)
            Dim rick As New Pilot("Rick", 33)
            Dim juliette As New Pilot("Juliette", 33)

            container.Store(New Car(john, "Ferrari"))
            container.Store(New Car(joanna, "Mercedes"))
            container.Store(New Car(jenny, "Volvo"))
            container.Store(New Car(rick, "Fiat"))
            container.Store(New Car(juliette, "Suzuki"))
        End Sub
    End Class
End Namespace