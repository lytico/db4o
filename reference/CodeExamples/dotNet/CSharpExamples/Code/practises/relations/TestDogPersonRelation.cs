using System.Linq;
using NUnit.Framework;

namespace Db4oDoc.Code.Practises.Relations
{
    public class TestDogPersonRelation
    {
        private static readonly Country USA = new Country("USA");

        [Test]
        public void AddDogToPerson()
        {
            var p = new Person("test", "test", USA);
            var dog = new Dog();
            p.AddOwnerShipOf(dog);

            assertIsConsistent(p, dog);
        }

        [Test]
        public void SetOwner()
        {
            var p = new Person("test", "test", USA);
            var dog = new Dog {Owner = p};

            assertIsConsistent(p, dog);
        }

        [Test]
        public void SettingOwnerRemovesOldOwner()
        {
            var dog = new Dog();
            var oldOwner = new Person("old-owner", "old-owner", USA);
            dog.Owner = oldOwner;
            var p = new Person("test", "test", USA);
            dog.Owner = p;

            assertIsConsistent(p, dog);
            Assert.AreEqual(0, oldOwner.OwnedDogs.Count());
        }

        [Test]
        public void RemoveOwner()
        {
            var dog = new Dog();
            var oldOwner = new Person("old-owner", "old-owner", USA);
            dog.Owner = oldOwner;
            dog.Owner = null;

            Assert.IsNull(dog.Owner);
            Assert.AreEqual(0, oldOwner.OwnedDogs.Count());
        }

        [Test]
        public void RemoveOwnedDog()
        {
            var dog = new Dog();
            var oldOwner = new Person("old-owner", "old-owner", USA);
            dog.Owner = (oldOwner);
            oldOwner.RemoveOwnerShipOf(dog);

            Assert.IsNull(dog.Owner);
            Assert.AreEqual(0, oldOwner.OwnedDogs.Count());
        }

        [Test]
        public void ChangeOwnerShip()
        {
            var dog = new Dog();
            var oldOwner = new Person("old-owner", "old-owner", USA);
            oldOwner.AddOwnerShipOf(dog);
            var p = new Person("test", "test", USA);
            p.AddOwnerShipOf(dog);

            assertIsConsistent(p, dog);
            Assert.AreEqual(0, oldOwner.OwnedDogs.Count());
        }


        private void assertIsConsistent(Person p, Dog dog)
        {
            Assert.AreSame(p, dog.Owner);
            Assert.IsTrue(p.OwnedDogs.Contains(dog));
        }
    }
}