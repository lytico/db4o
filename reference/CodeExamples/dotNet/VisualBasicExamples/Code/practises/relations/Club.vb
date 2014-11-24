Imports System.Collections.Generic

Namespace Db4oDoc.Code.Practises.Relations
    Friend Class Club
        ' #example: Bidirectional N-N relation
        Private ReadOnly m_members As ICollection(Of Person) = New HashSet(Of Person)()

        Public Sub AddMember(person As Person)
            If Not m_members.Contains(person) Then
                m_members.Add(person)
                person.Join(Me)
            End If
        End Sub

        Public Sub RemoveMember(person As Person)
            If m_members.Contains(person) Then
                m_members.Remove(person)
                person.Leave(Me)
            End If
        End Sub
        ' #end example

        Public ReadOnly Property Members() As IEnumerable(Of Person)
            Get
                Return m_members
            End Get
        End Property
    End Class
End Namespace
