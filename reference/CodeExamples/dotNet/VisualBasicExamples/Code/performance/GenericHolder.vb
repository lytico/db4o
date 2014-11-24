Imports Db4objects.Db4o.Config.Attributes

Namespace Db4oDoc.Code.Performance

    Public NotInheritable Class GenericHolder
        Private Sub New()
        End Sub
        Public Shared Function Create(Of T)(reference As T) As GenericHolder(Of T)
            Return New GenericHolder(Of T)(reference)
        End Function
    End Class
    Public Class GenericHolder(Of T)
        <Indexed()> _
        Private ReadOnly m_indexedReference As T

        Public Sub New(reference As T)
            Me.m_indexedReference = reference
        End Sub

        Public ReadOnly Property IndexedReference() As T
            Get
                Return m_indexedReference
            End Get
        End Property
    End Class
End Namespace
