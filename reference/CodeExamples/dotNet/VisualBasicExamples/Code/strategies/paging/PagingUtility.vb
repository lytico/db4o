Imports System.Collections.Generic

Namespace Db4oDoc.Code.Strategies.Paging
    Public NotInheritable Class PagingUtility
        Private Sub New()
        End Sub
        ' #example: Paging utility methods
        Public Shared Function Paging(Of T)(ByVal listToPage As IList(Of T), ByVal limit As Integer) As IList(Of T)
            Return Paging(listToPage, 0, limit)
        End Function

        Public Shared Function Paging(Of T)(ByVal listToPage As IList(Of T), ByVal start As Integer, ByVal limit As Integer) As IList(Of T)
            If start > listToPage.Count Then
                Throw New ArgumentException("You cannot start the paging outside the list." & " List-size: " & listToPage.Count & " start: " & start)
            End If
            Dim endPosition As Integer = calculateEnd(listToPage, start, limit)
            Dim list As IList(Of T) = New List(Of T)()
            For i As Integer = start To endPosition - 1
                list.Add(listToPage(i))
            Next
            Return list
        End Function

        Private Shared Function calculateEnd(Of T)(ByVal resultList As IList(Of T), _
                 ByVal start As Integer, ByVal limit As Integer) As Integer
            Dim endPosition As Integer = start + limit
            If endPosition >= resultList.Count Then
                Return resultList.Count
            End If
            Return endPosition
        End Function

        ' #end example
    End Class
End Namespace