Imports System.IO
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Diagnostic
Imports Db4objects.Db4o.Query
Imports NUnit.Framework

Namespace Db4oDoc.Code.Performance.Query.Soda
    Public Class BadPerformance
        Private Const DatabaseFile As String = "good-performance.db4o"
        Private Const NumberOfItems As Integer = 50000
        Private container As IObjectContainer
        Private ReadOnly rnd As New Random()


        <TestFixtureSetUp()> _
        Public Shared Sub RemoveDatabase()
            File.Delete(DatabaseFile)
        End Sub

        <SetUp()> _
        Public Sub PrepareData()
            container = Db4oEmbedded.OpenFile(newCfg(), DatabaseFile)
            If container.Query().Execute().Count = 0 Then
                storeTestData(container)
            End If
        End Sub

        <TearDown()> _
        Public Sub CleanUp()
            container.Dispose()
        End Sub

        <Test()> _
        Public Sub EqualsAcrossObjectFields()
            StopWatchUtil.Time(
                Function()
                    Dim criteria = Item.DataString(rnd.Next(NumberOfItems))

                    ' #example: Navigation across non concrete typed fields
                    ' The type of the 'indexedReference' is an object
                    ' Therefore the query engine cannot know the type and use that index
                    Dim query = container.Query()
                    query.Constrain(GetType(ObjectHolder))
                    query.Descend("m_indexedReference").Descend("m_indexedString").Constrain(criteria)
                    ' #end example


                    Dim result = query.Execute()
                    Console.WriteLine("Number of result items " & Convert.ToString(result.Count))

                End Function)
        End Sub


        <Test()> _
        Public Sub StartsWith()
            StopWatchUtil.Time(
                Function()
                    Dim criteria = Item.DataString(rnd.Next(NumberOfItems))
                    Dim query = container.Query()
                    query.Constrain(GetType(Item))
                    query.Descend("m_indexedString").Constrain(criteria).StartsWith(True)


                    Dim result = query.Execute()
                    Console.WriteLine("Number of result items " & Convert.ToString(result.Count))

                End Function)
        End Sub


        <Test()> _
        Public Sub EndsWith()
            StopWatchUtil.Time(
                Function()
                    Dim criteria = Item.DataString(rnd.Next(NumberOfItems))
                    Dim query = container.Query()
                    query.Constrain(GetType(Item))
                    query.Descend("m_indexedString").Constrain(criteria).EndsWith(True)

                    Dim result = query.Execute()
                    Console.WriteLine("Number of result items " & Convert.ToString(result.Count))

                End Function)
        End Sub

        <Test()> _
        Public Sub Contains()
            StopWatchUtil.Time(
                Function()
                    Dim criteria = Item.DataString(rnd.Next(NumberOfItems))

                    ' #example: Contains is slow
                    Dim query = container.Query()
                    query.Constrain(GetType(Item))
                    query.Descend("m_indexedString").Constrain(criteria).Contains()
                    ' #end example

                    Dim result = query.Execute()
                    Console.WriteLine("Number of result items " & Convert.ToString(result.Count))

                End Function)
        End Sub

        <Test()> _
        Public Sub LikeComparison()
            StopWatchUtil.Time(
                Function()
                    Dim criteria = Item.DataString(rnd.Next(NumberOfItems))
                    ' #example: Like is slow
                    Dim query = container.Query()
                    query.Constrain(GetType(Item))
                    query.Descend("m_indexedString").Constrain(criteria).Like()
                    ' #end example

                    Dim result = query.Execute()
                    Console.WriteLine("Number of result items " & Convert.ToString(result.Count))

                End Function)
        End Sub

        <Test()> _
        Public Sub QueryForItemInCollection()
            StopWatchUtil.Time(
                Function()
                    Dim itemToQueryFor As Item = LoadItemFromDatabase()

                    ' #example: Contains on collection
                    Dim query = container.Query()
                    query.Constrain(GetType(CollectionHolder))
                    query.Descend("m_items").Constrain(itemToQueryFor)
                    ' #end example

                    Dim result = query.Execute()
                    Console.WriteLine("Number of result items " & Convert.ToString(result.Count))

                End Function)
        End Sub

        <Test()> _
        Public Sub DescentIntoCollection()
            StopWatchUtil.Time(
                Function()
                    Dim criteria = Item.DataString(rnd.Next(NumberOfItems))

                    ' #example: Navigate into collection
                    Dim query = container.Query()
                    query.Constrain(GetType(CollectionHolder))
                    query.Descend("m_items").Descend("m_indexedString").Constrain(criteria)
                    ' #end example


                    Dim result = query.Execute()
                    Console.WriteLine("Number of result items " & Convert.ToString(result.Count))

                End Function)
        End Sub

        <Test()> _
        Public Sub SortingByIndexedField()
            StopWatchUtil.Time(
                Function()
                    ' #example: Sorting a huge result set
                    Dim query = container.Query()
                    query.Constrain(GetType(Item))
                    query.Descend("m_indexedString").OrderAscending()
                    ' #end example

                    Dim result = query.Execute()
                    Console.WriteLine("Number of result items " & Convert.ToString(result.Count))

                End Function)
        End Sub


        <Test()> _
        Public Sub Evalutions()
            StopWatchUtil.Time(
                Function()
                    ' #example: Evaluations
                    Dim query = container.Query()
                    query.Constrain(GetType(Item))
                    query.Descend("m_indexedString").Constrain(New OnlyAbcItemsEvaluation())
                    ' #end example

                    Dim result = query.Execute()
                    Console.WriteLine("Number of result items " & Convert.ToString(result.Count))

                End Function)
        End Sub

        ' #example: Evaluation class
        Friend Class OnlyAbcItemsEvaluation
            Implements IEvaluation
            Public Sub Evaluate(candidate As ICandidate) Implements IEvaluation.Evaluate
                If TypeOf candidate.GetObject() Is String Then
                    Dim value = DirectCast(candidate.GetObject(), String)
                    If value.Equals("abc") Then
                        candidate.Include(True)
                    End If
                End If
            End Sub
        End Class
        ' #end example

        Private Function LoadItemFromDatabase() As Item
            Dim criteria = Item.DataString(rnd.Next(NumberOfItems))
            Dim itemQuery = container.Query()
            itemQuery.Constrain(GetType(Item))
            itemQuery.Descend("m_indexedString").Constrain(criteria)
            Return DirectCast(itemQuery.Execute()(0), Item)
        End Function

        Private Function NewCfg() As IEmbeddedConfiguration
            Dim cfg = Db4oEmbedded.NewConfiguration()
            cfg.Common.Diagnostic.AddListener(New DiagnosticToConsole())
            Return cfg
        End Function

        Private Sub StoreTestData(container As IObjectContainer)
            For i As Integer = 0 To NumberOfItems - 1
                Dim item As New Item(i)
                container.Store(ObjectHolder.Create(item))
                container.Store(CollectionHolder.Create(item, New Item(NumberOfItems + i), New Item(2 * NumberOfItems + i)))
            Next
        End Sub
    End Class
End Namespace
