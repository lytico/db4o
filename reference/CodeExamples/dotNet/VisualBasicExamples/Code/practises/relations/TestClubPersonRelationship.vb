Imports System.Linq
Imports NUnit.Framework

Namespace Db4oDoc.Code.Practises.Relations
    Public Class TestClubPersonRelationship
        Private Shared ReadOnly USA As New Country("USA")

        <Test()> _
        Public Sub JoinClub()
            Dim p = New Person("test", "test", USA)
            Dim theClub = New Club()
            p.Join(theClub)

            assertIsConsistent(p, theClub)
        End Sub

        <Test()> _
        Public Sub AddMember()
            Dim p = New Person("test", "test", USA)
            Dim theClub = New Club()
            theClub.AddMember(p)

            assertIsConsistent(p, theClub)
        End Sub

        <Test()> _
        Public Sub RemoveMember()
            Dim p1 = New Person("test", "test", USA)
            Dim p2 = New Person("test", "test", USA)
            Dim theClub = New Club()
            theClub.AddMember(p1)
            theClub.AddMember(p2)
            theClub.RemoveMember(p1)

            assertIsConsistent(p2, theClub)
            Assert.AreEqual(0, p1.MemberOf.Count())
            Assert.AreEqual(1, theClub.Members.Count())
        End Sub

        <Test()> _
        Public Sub LeaveClub()
            Dim p1 = New Person("test", "test", USA)
            Dim p2 = New Person("test", "test", USA)
            Dim theClub = New Club()
            theClub.AddMember(p1)
            theClub.AddMember(p2)
            p1.Leave(theClub)

            assertIsConsistent(p2, theClub)
            Assert.AreEqual(0, p1.MemberOf.Count())
            Assert.AreEqual(1, theClub.Members.Count())
        End Sub

        <Test()> _
        Public Sub MulitpleClubs()
            Dim p1 = New Person("test", "test", USA)
            Dim p2 = New Person("test", "test", USA)
            Dim club1 = New Club()
            Dim club2 = New Club()
            p1.Join(club1)
            p1.Join(club2)
            club1.AddMember(p2)
            club2.AddMember(p2)

            assertIsConsistent(p1, club1)
            assertIsConsistent(p1, club2)
            assertIsConsistent(p2, club1)
            assertIsConsistent(p1, club2)
        End Sub

        Private Sub assertIsConsistent(person As Person, club As Club)
            Assert.IsTrue(person.MemberOf.Contains(club))
            Assert.IsTrue(club.Members.Contains(person))
        End Sub
    End Class
End Namespace
