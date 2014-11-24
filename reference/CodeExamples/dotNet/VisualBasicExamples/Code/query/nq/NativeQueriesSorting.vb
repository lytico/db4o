Imports System.Collections
Imports System.Collections.Generic
Imports System.IO
Imports Db4objects.Db4o

Namespace Db4oDoc.Code.Query.NativeQueries
    Public Class NativeQueriesSorting
        Private Const DatabaseFile As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            CleanUp()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                StoreData(container)

                NativeQuerySorting(container)
            End Using
        End Sub


        Private Shared Sub NativeQuerySorting(ByVal container As IObjectContainer)
            ' #example: Native query with sorting
            Dim pilots As IList(Of Pilot) = container.Query(Of Pilot)(AddressOf QueryForAdults, AddressOf SortByName)
            ' #end example

            ListResult(pilots)
        End Sub

        ' #example: Query and sort function
        Private Shared Function QueryForAdults(ByVal pilot As Pilot) As Boolean
            Return pilot.Age > 18
        End Function

        Private Shared Function SortByName(ByVal pilot1 As Pilot, ByVal pilot2 As Pilot) As Integer
            Return pilot1.Name.CompareTo(pilot2.Name)
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

            container.Store(New Car(john, "Ferrari"))
            container.Store(New Car(joanna, "Mercedes"))
            container.Store(New Car(jenny, "Volvo"))
            container.Store(New Car(rick, "Fiat"))
        End Sub
    End Class

End Namespace