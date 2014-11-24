Imports Db4objects.Db4o.Config.Attributes

Namespace Db4oDoc.Code.Performance
    Public Class ObjectHolder
        <Indexed()> _
        Private ReadOnly m_indexedReference As Object

        Public Sub New(item As Object)
            Me.m_indexedReference = item
        End Sub

        Public Shared Function Create(reference As Object) As ObjectHolder
            Return New ObjectHolder(reference)
        End Function

        Public ReadOnly Property IndexedReference() As Object
            Get
                Return m_indexedReference
            End Get
        End Property
    End Class
End Namespace
