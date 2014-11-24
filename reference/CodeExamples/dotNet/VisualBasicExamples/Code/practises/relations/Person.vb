Imports System.Collections.Generic

Namespace Db4oDoc.Code.Practises.Relations
    Friend Class Person
        Private sirname As String
        Private firstname As String

        Public Sub New(sirname As String, firstname As String, bornIn As Country)
            Me.sirname = sirname
            Me.firstname = firstname
            Me.m_bornIn = bornIn
        End Sub


        ' #example: Simple 1-n relation. Navigating from the person to the countries
        ' Optionally we can index this field, when we want to find all people for a certain country
        Private m_bornIn As Country

        Public ReadOnly Property BornIn() As Country
            Get
                Return m_bornIn
            End Get
        End Property
        ' #end example

        ' #example: One directional N-N relation
        Private ReadOnly citizenIn As ICollection(Of Country) = New HashSet(Of Country)()

        Public Sub RemoveCitizenship(o As Country)
            citizenIn.Remove(o)
        End Sub

        Public Sub AddCitizenship(country As Country)
            citizenIn.Add(country)
        End Sub
        ' #end example

        ' #example: Bidirectional 1-N relations. The person has dogs
        Private ReadOnly owns As ICollection(Of Dog) = New HashSet(Of Dog)()

        ' The add and remove method ensure that the relations is always consistent
        Public Sub AddOwnerShipOf(dog As Dog)
            owns.Add(dog)
            dog.Owner = Me
        End Sub

        Public Sub RemoveOwnerShipOf(dog As Dog)
            owns.Remove(dog)
            dog.Owner = Nothing
        End Sub

        Public ReadOnly Property OwnedDogs() As IEnumerable(Of Dog)
            Get
                Return owns
            End Get
        End Property
        ' #end example

        ' #example: Bidirectional N-N relation
        Private ReadOnly memberIn As ICollection(Of Club) = New HashSet(Of Club)()

        Public Sub Join(club As Club)
            If Not memberIn.Contains(club) Then
                memberIn.Add(club)
                club.AddMember(Me)
            End If
        End Sub

        Public Sub Leave(club As Club)
            If memberIn.Contains(club) Then
                memberIn.Remove(club)
                club.RemoveMember(Me)
            End If
        End Sub

        Public ReadOnly Property MemberOf() As IEnumerable(Of Club)
            Get
                Return memberIn
            End Get
        End Property
        ' #end example
    End Class
End Namespace
