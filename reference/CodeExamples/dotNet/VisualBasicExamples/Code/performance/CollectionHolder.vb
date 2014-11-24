Imports System.Collections.Generic
Imports System.Linq

Namespace Db4oDoc.Code.Performance
    Public Class CollectionHolder
        Private m_items As IList(Of Item)

        Private Sub New(items As IList(Of Item))
            Me.m_items = items
        End Sub

        Public Shared Function Create(ParamArray itemsToAdd As Item()) As CollectionHolder
            Return New CollectionHolder(itemsToAdd.ToList())
        End Function

        Public ReadOnly Property Items() As IList(Of Item)
            Get
                Return m_items
            End Get
        End Property
    End Class
End Namespace
