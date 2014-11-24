using System.Linq;
using NUnit.Framework;

namespace Db4oDoc.Code.Practises.Relations
{
    public class TestClubPersonRelationship
    {
        private static readonly Country USA = new Country("USA");

        [Test]
        public void JoinClub()
        {
            var p = new Person("test", "test", USA);
            var theClub = new Club();
            p.Join(theClub);

            assertIsConsistent(p, theClub);
        }

        [Test]
        public void AddMember()
        {
            var p = new Person("test", "test", USA);
            var theClub = new Club();
            theClub.AddMember(p);

            assertIsConsistent(p, theClub);
        }

        [Test]
        public void RemoveMember()
        {
            var p1 = new Person("test", "test", USA);
            var p2 = new Person("test", "test", USA);
            var theClub = new Club();
            theClub.AddMember(p1);
            theClub.AddMember(p2);
            theClub.RemoveMember(p1);

            assertIsConsistent(p2, theClub);
            Assert.AreEqual(0, p1.MemberOf.Count());
            Assert.AreEqual(1, theClub.Members.Count());
        }

        [Test]
        public void LeaveClub()
        {
            var p1 = new Person("test", "test", USA);
            var p2 = new Person("test", "test", USA);
            var theClub = new Club();
            theClub.AddMember(p1);
            theClub.AddMember(p2);
            p1.Leave(theClub);

            assertIsConsistent(p2, theClub);
            Assert.AreEqual(0, p1.MemberOf.Count());
            Assert.AreEqual(1, theClub.Members.Count());
        }

        [Test]
        public void MulitpleClubs()
        {
            var p1 = new Person("test", "test", USA);
            var p2 = new Person("test", "test", USA);
            var club1 = new Club();
            var club2 = new Club();
            p1.Join(club1);
            p1.Join(club2);
            club1.AddMember(p2);
            club2.AddMember(p2);

            assertIsConsistent(p1, club1);
            assertIsConsistent(p1, club2);
            assertIsConsistent(p2, club1);
            assertIsConsistent(p1, club2);
        }

        private void assertIsConsistent(Person person, Club club)
        {
            Assert.IsTrue(person.MemberOf.Contains(club));
            Assert.IsTrue(club.Members.Contains(person));
        }
    }
}