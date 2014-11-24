Imports Db4objects.Db4o.Config.Attributes

Namespace Db4oDoc.Code.Performance
    Public Class ItemHolder
        <Indexed()> _
        Private ReadOnly m_indexedReference As Item

        Public Sub New(item As Item)
            Me.m_indexedReference = item
        End Sub

        Public Shared Function Create(reference As Item) As ItemHolder
            Return New ItemHolder(reference)
        End Function

        Public ReadOnly Property IndexedReference() As Item
            Get
                Return m_indexedReference
            End Get
        End Property
    End Class
End Namespace
