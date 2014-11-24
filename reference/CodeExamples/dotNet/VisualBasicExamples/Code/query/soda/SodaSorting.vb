Imports System.Collections
Imports System.IO
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Query

Namespace Db4oDoc.Code.Query.Soda
    Public Class SodaSorting
        Private Const DatabaseFile As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            CleanUp()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                StoreData(container)

                SortingOnField(container)
                SortingOnMultipleFields(container)
                CustomOrder(container)
            End Using
        End Sub

        Private Shared Sub SortingOnField(ByVal container As IObjectContainer)
            Console.WriteLine("Order by a field")
            ' #example: Order by a field
            Dim query As IQuery = container.Query()
            query.Constrain(GetType(Pilot))
            query.Descend("name").OrderAscending()

            Dim result As IObjectSet = query.Execute()
            ' #end example
            ListResult(result)
        End Sub

        Private Shared Sub SortingOnMultipleFields(ByVal container As IObjectContainer)
            Console.WriteLine("Order by multiple fields")
            ' #example: Order by multiple fields
            Dim query As IQuery = container.Query()
            query.Constrain(GetType(Pilot))
            query.Descend("age").OrderAscending()
            query.Descend("name").OrderAscending()

            Dim result As IObjectSet = query.Execute()
            ' #end example
            ListResult(result)
        End Sub

        Private Shared Sub CustomOrder(ByVal container As IObjectContainer)
            Console.WriteLine("Order by your comparator")
            ' #example: Order by your comparator
            Dim query As IQuery = container.Query()
            query.Constrain(GetType(Pilot))
            query.SortBy(New NameLengthComperator())

            Dim result As IObjectSet = query.Execute()
            ' #end example
            ListResult(result)
        End Sub

        ' #example: The string length comperator
        Private Class NameLengthComperator
            Implements IQueryComparator
            Public Function Compare(ByVal first As Object, ByVal second As Object) As Integer _
                Implements IQueryComparator.Compare

                Dim p1 As Pilot = DirectCast(first, Pilot)
                Dim p2 As Pilot = DirectCast(second, Pilot)
                ' sort by string-length
                Return Math.Sign(p1.Name.Length - p2.Name.Length)
            End Function
        End Class
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
            container.Store(New Pilot("John", 42))
            container.Store(New Pilot("Joanna", 45))
            container.Store(New Pilot("Brigit", 59))
            container.Store(New Pilot("Jenny", 21))
            container.Store(New Pilot("Rick", 33))
            container.Store(New Pilot("Jolanda", 33))
            container.Store(New Pilot("Chris", 22))
            container.Store(New Pilot("John", 33))
            container.Store(New Pilot("Raphael", 34))
            container.Store(New Pilot("Paul", 61))
            container.Store(New Pilot("Li", 43))
        End Sub
    End Class
End Namespace