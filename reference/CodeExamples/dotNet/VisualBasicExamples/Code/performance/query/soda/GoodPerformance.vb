Imports System.IO
Imports Db4objects.Db4o
Imports NUnit.Framework

Namespace Db4oDoc.Code.Performance.Query.Soda
    <TestFixture()> _
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
                    Dim query = container.Query()
                    query.Constrain(GetType(Item))
                    query.Descend("m_indexedString").Constrain(criteria)
                    ' #end example

                    Dim result = query.Execute()
                    Console.WriteLine("Number of result items " & Convert.ToString(result.Count))

                End Function)
        End Sub

        <Test()> _
        Public Sub NotEquals()
            StopWatchUtil.Time(
                Function()
                    Dim criteria = Item.DataString(rnd.Next(NumberOfItems))

                    ' #example: Not equals on indexed field
                    Dim query = container.Query()
                    query.Constrain(GetType(Item))
                    query.Descend("m_indexedString").Constrain(criteria).Not()
                    ' #end example

                    Dim result = query.Execute()
                    Console.WriteLine("Number of result items " & Convert.ToString(result.Count))

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
                    Dim query = container.Query()
                    query.Constrain(GetType(ItemHolder))
                    query.Descend("m_indexedReference").Descend("m_indexedString").Constrain(criteria)
                    ' #end example

                    Dim result = query.Execute()
                    Console.WriteLine("Number of result items " & Convert.ToString(result.Count))

                End Function)
        End Sub

        <Test()> _
        Public Sub SearchByReference()
            StopWatchUtil.Time(
                Function()
                    ' #example: Query by reference
                    Dim item As Item = LoadItemFromDatabase()

                    Dim query = container.Query()
                    query.Constrain(GetType(ItemHolder))
                    query.Descend("m_indexedReference").Constrain(item)
                    ' #end example

                    Dim result = query.Execute()
                    Console.WriteLine("Number of result items " & Convert.ToString(result.Count))

                End Function)
        End Sub


        <Test()> _
        Public Sub BiggerThan()
            StopWatchUtil.Time(
                Function()
                    Dim criteria As Integer = rnd.Next(NumberOfItems)

                    ' #example: Bigger than
                    Dim query = container.Query()
                    query.Constrain(GetType(Item))
                    query.Descend("m_indexNumber").Constrain(criteria).Greater()
                    ' #end example

                    Dim result = query.Execute()
                    Console.WriteLine("Number of result items " & Convert.ToString(result.Count))

                End Function)
        End Sub

        <Test()> _
        Public Sub NotBiggerThan()
            StopWatchUtil.Time(
                Function()
                    Dim criteria As Integer = rnd.Next(NumberOfItems)

                    Dim query = container.Query()
                    query.Constrain(GetType(Item))
                    query.Descend("m_indexNumber").Constrain(criteria).Not().Greater()

                    Dim result = query.Execute()
                    Console.WriteLine("Number of result items " & Convert.ToString(result.Count))

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
                    Dim query = container.Query()
                    query.Constrain(GetType(Item))
                    query.Descend("m_indexNumber").Constrain(biggerThanThis).Greater() _
                                  .And(query.Descend("indexNumber").Constrain(smallerThanThis).Smaller())
                    ' #end example

                    Dim result = query.Execute()
                    Console.WriteLine("Number of result items " & Convert.ToString(result.Count))

                End Function)
        End Sub

        <Test()> _
        Public Sub FindADate()
            StopWatchUtil.Time(
                Function()
                    Dim coparisonDate = New DateTime(rnd.Next(NumberOfItems))

                    ' #example: Date comparisons are also fast
                    Dim query = container.Query()
                    query.Constrain(GetType(Item))
                    query.Descend("m_indexDate").Constrain(coparisonDate)
                    ' #end example

                    Dim result = query.Execute()
                    Console.WriteLine("Number of result items " & Convert.ToString(result.Count))

                End Function)
        End Sub

        <Test()> _
        Public Sub NewerData()
            StopWatchUtil.Time(
                Function()
                    Dim coparisonDate = New DateTime(rnd.Next(NumberOfItems))

                    ' #example: Find a newer date
                    Dim query = container.Query()
                    query.Constrain(GetType(Item))
                    query.Descend("m_indexDate") _
                                .Constrain(coparisonDate).Greater()
                    ' #end example

                    Dim result = query.Execute()
                    Console.WriteLine("Number of result items " & Convert.ToString(result.Count))

                End Function)
        End Sub


        Private Sub StoreTestData(container As IObjectContainer)
            For i As Integer = 0 To NumberOfItems - 1
                container.Store(ItemHolder.Create(New Item(i)))
            Next
        End Sub


        Private Function LoadItemFromDatabase() As Item
            Dim criteria = Item.DataString(rnd.Next(NumberOfItems))
            Dim itemQuery = container.Query()
            itemQuery.Constrain(GetType(Item))
            itemQuery.Descend("m_indexedString").Constrain(criteria)
            Return DirectCast(itemQuery.Execute()(0), Item)
        End Function
    End Class
End Namespace
