Imports System.IO
Imports System.Linq
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Linq
Imports NUnit.Framework

Namespace Db4oDoc.Code.Performance.Query.Linq
    Public Class BadPerformance
        Private Const DatabaseFile As String = "good-performance.db4o"
        Private Const NumberOfItems As Integer = 200000
        Private container As IObjectContainer
        Private ReadOnly rnd As New Random()


        <TestFixtureSetUp()> _
        Public Shared Sub RemoveDatabase()
            File.Delete(DatabaseFile)
        End Sub

        <SetUp()> _
        Public Sub PrepareData()
            container = Db4oEmbedded.OpenFile(DatabaseFile)
            If container.Query().Execute().Count = 0 Then
                StoreTestData(container)
            End If
            WarmUpLinq(container)
        End Sub

        <TearDown()> _
        Public Sub CleanUp()
            container.Dispose()
        End Sub

        <Test()> _
        Public Sub Contains()
            StopWatchUtil.Time(
                Function()
                    Dim criteria = Item.DataString(rnd.[Next](NumberOfItems))

                    ' #example: Contains is slow
                    Dim result = From i As Item In container _
                            Where i.IndexedString.Contains(criteria) _
                            Select i
                    ' #end example

                    Console.WriteLine("Number of result items {0}", result.Count())

                End Function)
        End Sub

        <Test()> _
        Public Sub QueryForItemInCollection()
            StopWatchUtil.Time(
                Function()
                    Dim itemToQueryFor As Item = LoadItemFromDatabase()

                    ' #example: Contains on collection
                    Dim result = From h As CollectionHolder In container _
                        Where h.Items.Contains(itemToQueryFor) _
                        Select h
                    ' #end example

                    Console.WriteLine("Number of result items {0}", result.Count())

                End Function)
        End Sub

        <Test()> _
        Public Sub PropertiesWithSideEffects()
            StopWatchUtil.Time(
                Function()
                    Dim criteria = Item.DataString(rnd.[Next](NumberOfItems))

                    ' #example: Complex property
                    Dim result = From i As Item In container _
                            Where i.PropertyWhichFiresEvent = criteria _
                            Select i
                    ' #end example

                    Console.WriteLine("Number of result items {0}", result.Count())

                End Function)
        End Sub

        <Test()> _
        Public Sub CallingAMethod()
            StopWatchUtil.Time(
                Function()
                    ' #example: Calling a method
                    Dim result = From i As Item In container _
                            Where i.ComplexMethod() _
                            Select i
                    ' #end example

                    Console.WriteLine("Number of result items {0}", result.Count())

                End Function)
        End Sub

        <Test()> _
        Public Sub AdvancedLinqQueries()
            StopWatchUtil.Time(
                Function()
                    Dim criteria = Item.DataString(rnd.[Next](NumberOfItems))

                    ' #example: Nagivating into collection
                    Dim result = From h As CollectionHolder In container _
                                            Where h.Items.Any(Function(i) i.IndexedString = criteria) _
                                            Select h
                    ' #end example

                    Console.WriteLine("Number of result items {0}", result.Count())

                End Function)
        End Sub

        Private Function LoadItemFromDatabase() As Item
            Dim criteria = Item.DataString(rnd.[Next](NumberOfItems))
            Return (From i As Item In container Where i.IndexedString = criteria Select i).First()
        End Function

        Private Sub StoreTestData(container As IObjectContainer)
            For i As Integer = 0 To NumberOfItems - 1
                Dim item = New Item(i)
                container.Store(ItemHolder.Create(item))
                container.Store(GenericHolder.Create(item))
                container.Store(CollectionHolder.Create(item, New Item(NumberOfItems + i), New Item(2 * NumberOfItems + i)))
            Next
            container.Commit()
        End Sub

        Private Function WarmUpLinq(container As IObjectContainer) As Integer
            Dim result = (From i As Item In container Where i.IndexedString = "No-Match" Select i).ToList()
            Return result.Count
        End Function
    End Class
End Namespace
