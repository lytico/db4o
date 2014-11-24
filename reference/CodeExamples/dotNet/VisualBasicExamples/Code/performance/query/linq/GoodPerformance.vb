Imports System.IO
Imports System.Linq
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Linq
Imports NUnit.Framework

Namespace Db4oDoc.Code.Performance.Query.Linq
    Public Class GoodPerformance
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
        Public Sub EqualsOnIndexedField()
            StopWatchUtil.Time(
             Function()
                 Dim criteria = Item.DataString(rnd.Next(NumberOfItems))

                 ' #example: Equals on indexed field
                 Dim result = From i As Item In container _
                   Where i.IndexedString = criteria _
                   Select i
                 ' #end example

                 Console.WriteLine("Number of result items {0}", result.Count())

             End Function)
        End Sub

        <Test()> _
        Public Sub NotEquals()
            StopWatchUtil.Time(
                Function()
                    Dim criteria = Item.DataString(rnd.Next(NumberOfItems))

                    ' #example: Not equals
                    Dim result = From i As Item In container _
                            Where i.IndexedString <> criteria _
                            Select i
                    ' #end example

                    Console.WriteLine("Number of result items {0}", result.Count())

                End Function)
        End Sub

        <Test()> _
        Public Sub EqualsAcrossIndexedFields()
            StopWatchUtil.Time(
                Function()
                    Dim criteria = Item.DataString(rnd.Next(NumberOfItems))

                    ' #example: Equals across indexed fields
                    ' Note that the type of the 'indexedReference' has to the specific type
                    ' which holds the 'indexedString'
                    Dim result = From h As ItemHolder In container _
                            Where h.IndexedReference.IndexedString = criteria _
                            Select h
                    ' #end example

                    Console.WriteLine("Number of result items {0}", result.Count())

                End Function)
        End Sub

        <Test()> _
        Public Sub SearchByReference()
            StopWatchUtil.Time(
                Function()
                    ' #example: Query by reference
                    Dim item As Item = LoadItemFromDatabase()

                    ' #example: Query by reference
                    ' Note that the type of the 'indexedReference' has to the specific type
                    ' which holds the 'indexedString'
                    Dim result = From h As ItemHolder In container _
                            Where h.IndexedReference Is item _
                            Select h
                    ' #end example

                    Console.WriteLine("Number of result items {0}", result.Count())

                End Function)
        End Sub

        <Test()> _
        Public Sub SearchByGenericField()
            StopWatchUtil.Time(
                Function()
                    Dim item As Item = LoadItemFromDatabase()

                    Dim result = From h As GenericHolder(Of Item) In container _
                            Where h.IndexedReference Is item _
                             Select h

                    Console.WriteLine("Number of result items {0}", result.Count())

                End Function)
        End Sub


        <Test()> _
        Public Sub BiggerThan()
            StopWatchUtil.Time(
                Function()
                    Dim criteria As Integer = rnd.Next(NumberOfItems)

                    ' #example: Bigger than
                    Dim result = From i As Item In container _
                            Where i.IndexNumber > criteria _
                            Select i
                    ' #end example

                    Console.WriteLine("Number of result items {0}", result.Count())

                End Function)
            StopWatchUtil.Time(
                Function()
                    Dim criteria As Integer = rnd.Next(NumberOfItems)

                    ' #example: Bigger than
                    Dim result = From i As Item In container _
                            Where i.IndexNumber > criteria _
                            Select i
                    ' #end example

                    Console.WriteLine("Number of result items {0}", result.Count())

                End Function)
        End Sub

        <Test()> _
        Public Sub InBetween()
            StopWatchUtil.Time(
                Function()
                    Dim criteria As Integer = rnd.Next(NumberOfItems)
                    Dim biggerThanThis As Integer = criteria - 10
                    Dim smallerThanThis As Integer = criteria + 10

                    ' #example: In between
                    Dim result = From i As Item In container _
                            Where i.IndexNumber > biggerThanThis _
                            AndAlso i.IndexNumber < smallerThanThis _
                            Select i
                    ' #end example  

                    Console.WriteLine("Number of result items {0}", result.Count())

                End Function)
        End Sub

        <Test()> _
        Public Sub FindADate()
            StopWatchUtil.Time(
                Function()
                    Dim dateToFind = New DateTime(rnd.Next(NumberOfItems))

                    ' #example: Date comparisons are also fast
                    Dim result = From i As Item In container _
                            Where i.IndexDate = dateToFind _
                            Select i
                    ' #end example

                    Console.WriteLine("Number of result items {0}", result.Count())

                End Function)
        End Sub

        <Test()> _
        Public Sub NewerData()
            StopWatchUtil.Time(Function()
                                   Dim newerThanThis = New DateTime(rnd.Next(NumberOfItems))

                                   ' #example: Find a newer date
                                   Dim result = From i As Item In container _
                                           Where i.IndexDate > newerThanThis _
                                           Select i
                                   ' #end example

                                   Console.WriteLine("Number of result items {0}", result.Count())

                               End Function)
        End Sub

        Private Function LoadItemFromDatabase() As Item
            Dim criteria = Item.DataString(rnd.Next(NumberOfItems))
            Return (From i As Item In container Where i.IndexedString = criteria Select i).First()
        End Function

        Private Sub StoreTestData(container As IObjectContainer)
            For i As Integer = 0 To NumberOfItems - 1
                Dim item = New Item(i)
                container.Store(ItemHolder.Create(item))
                container.Store(GenericHolder.Create(item))
            Next
            container.Commit()
        End Sub

        Private Function WarmUpLinq(container As IObjectContainer) As Integer
            Dim result = (From i As Item In container Where i.IndexedString = "No-Match" Select i).ToList()
            Return result.Count
        End Function
    End Class
End Namespace
