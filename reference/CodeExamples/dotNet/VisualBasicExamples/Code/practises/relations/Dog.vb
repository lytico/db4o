Imports System.Linq

Namespace Db4oDoc.Code.Practises.Relations
    Friend Class Dog
        ' #example: Bidirectional 1-N relations. The dog has a owner
        Private m_owner As Person

        Public Property Owner() As Person
            Get
                Return m_owner
            End Get
            Set(value As Person)
                ' This setter ensures that the model is always consistent
                If Me.m_owner IsNot Nothing Then
                    Dim oldOwner As Person = Me.m_owner
                    Me.m_owner = Nothing
                    oldOwner.RemoveOwnerShipOf(Me)
                End If
                If value IsNot Nothing AndAlso Not value.OwnedDogs.Contains(Me) Then
                    value.AddOwnerShipOf(Me)
                End If
                Me.m_owner = value
            End Set
        End Property
        ' #end example
    End Class
End Namespace
