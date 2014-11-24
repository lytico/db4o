Imports System.Collections.Generic
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Events
Imports Db4objects.Db4o.IO
Imports NUnit.Framework

Namespace Db4oDoc.Code.Strategies.Paging
    Public Class TestPagingUtility
        <Test()> _
        Public Sub LimitsEntries()
            Dim input = New List(Of Integer)
            AddRange(input, 5)
            Dim result = PagingUtility.Paging(input, 2)
            AssertContains(result, 1, 2)
        End Sub

        Private Sub AddRange(ByVal list As List(Of Integer), ByVal ending As Integer)
            For index As Integer = 1 To ending
                list.Add(index)
            Next

        End Sub

        <Test()> _
        Public Sub PageTo()
            Dim input = New List(Of Integer)
            AddRange(input, 5)
            Dim result = PagingUtility.Paging(input, 2, 2)
            AssertContains(result, 3, 4)
        End Sub

        <Test()> _
        Public Sub ToLargeLimitResultInSizeOfList()
            Dim input = New List(Of Integer)
            AddRange(input, 1)
            Dim result = PagingUtility.Paging(input, 2)
            AssertContains(result, 1)
        End Sub


        <Test()> _
        <ExpectedException(GetType(ArgumentException))> _
        Public Sub StartOutOfRange()
            Dim input = New List(Of Integer)
            AddRange(input, 1)
            PagingUtility.Paging(input, 2, 1)
        End Sub


        <Test()> _
        Public Sub StartAtTheEndReturnsEmpty()
            Dim input = New List(Of Integer)
            AddRange(input, 3)
            Dim result = PagingUtility.Paging(input, 3, 10)
            Assert.AreEqual(0, result.Count)
        End Sub

        <Test()> _
        Public Sub DoesOnlyActivateNeededItems()
            Dim memoryFileSystem As New MemoryStorage()
            StoreItems(memoryFileSystem)
            Dim container As IObjectContainer = NewDB(memoryFileSystem)
            Dim counter = ActivationCounter(container)

            ' #example: Use the paging utility
            Dim queryResult As IList(Of StoredItems) = container.Query(Of StoredItems)()
            Dim pagedResult As IList(Of StoredItems) = PagingUtility.Paging(queryResult, 2, 2)
            ' #end example
            For Each storedItems As StoredItems In pagedResult
            Next
            Assert.AreEqual(2, counter.Value)
        End Sub

        Private Shared Function ActivationCounter(ByVal container As IObjectContainer) As MutableInteger
            Dim counter = New MutableInteger()
            AddHandler EventRegistryFactory.ForObjectContainer(container).Activating, AddressOf counter.Increment
            Return counter
        End Function

        Private Shared Sub StoreItems(ByVal memoryFileSystem As MemoryStorage)
            Using container As IObjectContainer = NewDB(memoryFileSystem)
                For i As Integer = 0 To 9
                    container.Store(New StoredItems())
                Next
            End Using
        End Sub

        Private Shared Function NewDB(ByVal memoryFileSystem As MemoryStorage) As IObjectContainer
            Dim configuration = Db4oEmbedded.NewConfiguration()
            configuration.File.Storage = memoryFileSystem
            Return Db4oEmbedded.OpenFile(configuration, "MemoryDB:")
        End Function


        Private Shared Sub AssertContains(Of T)(ByVal result As IList(Of T), ByVal ParamArray entries As T())
            For Each entry As T In entries
                Assert.IsTrue(result.Contains(entry))
            Next
        End Sub

        Private Class MutableInteger
            Private m_value As Integer

            Public Function Increment() As Integer
                m_value += 1
                Return m_value
            End Function

            Public ReadOnly Property Value() As Integer
                Get
                    Return m_value
                End Get
            End Property

        End Class

        Private Class StoredItems
        End Class
    End Class
End Namespace