using System;
using System.Collections.Generic;
using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Collections;

namespace Db4oDoc.Code.Collections.BigSet
{
    public class BigSetExample
    {
        private const string DatabaseFileName = "database.db4o";
        private const int PopulationSize = 10000;

        public static void Main(string[] args)
        {
            CleanUp();

            StoreBigSet();
            CheckInBigSet();
            BigSetIsByIdentity();

            CleanUp();
        }


        private static void StoreBigSet()
        {
            using (IObjectContainer container = OpenDatabase())
            {
                City city = CreateCity(container);
                container.Store(city);
                StoreOtherPeople(container);
                container.Commit();
            }
        }

        private static void CheckInBigSet()
        {
            using (IObjectContainer container = OpenDatabase())
            {
                City city = container.Query<City>()[0];
                Console.WriteLine("City's population " + city.Population);

                CheckAFewPersons(container, city);
            }
        }

        private static void BigSetIsByIdentity()
        {
            using (IObjectContainer container = OpenDatabase())
            {
                City city = container.Query<City>()[0];

                // #example: Note that the big-set compares by identity, not by equality
                Person aCitizen;
                using (IEnumerator<Person> aCitizenEnumerator = city.Citizen.GetEnumerator())
                {
                    aCitizenEnumerator.MoveNext();
                    aCitizen = aCitizenEnumerator.Current;
                }
                Console.WriteLine("The big-set uses the identity, not equality of an object");
                Console.WriteLine("Therefore it .contains() on the same person-object is "
                                  + city.Citizen.Contains(aCitizen));
                Person equalPerson = new Person(aCitizen.Name);
                Console.WriteLine("Therefore it .contains() on a equal person-object is "
                                  + city.Citizen.Contains(equalPerson));
                // #end example
            }
        }

        private static City CreateCity(IObjectContainer container)
        {
            // #example: Crate a big-set instance
            ICollection<Person> citizen = CollectionFactory.ForObjectContainer(container).NewBigSet<Person>();
            // now you can use the big-set like a normal set:
            citizen.Add(new Person("Citizen Kane"));
            // #end example
            for (int i = 0; i < PopulationSize; i++)
            {
                citizen.Add(new Person("Citizen No " + i));
            }
            return new City(citizen);
        }

        private static void StoreOtherPeople(IObjectContainer container)
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                container.Store(new Person("Citizen No " + i));
            }
        }

        private static void CheckAFewPersons(IObjectContainer container, City city)
        {
            Random random = new Random();
            IList<Person> persons = container.Query<Person>();
            int personCount = persons.Count;
            for (int i = 0; i < 10; i++)
            {
                Person aPerson = persons[random.Next(personCount)];
                PrintCitizenStatus(city, aPerson);
            }
        }

        private static void PrintCitizenStatus(City city, Person aPerson)
        {
            if (city.IsCitizen(aPerson))
            {
                Console.WriteLine(aPerson + " is a citizen");
            }
            else
            {
                Console.WriteLine(aPerson + " isn't a citizen");
            }
        }


        private static IObjectContainer OpenDatabase()
        {
            return Db4oEmbedded.OpenFile(DatabaseFileName);
        }

        private static void CleanUp()
        {
            File.Delete(DatabaseFileName);
        }
    }

    internal class City
    {
        private readonly ICollection<Person> citizen;

        public City(ICollection<Person> citizen)
        {
            this.citizen = citizen;
        }

        public bool IsCitizen(Person person)
        {
            return citizen.Contains(person);
        }

        public ICollection<Person> Citizen
        {
            get { return citizen; }
        }

        public int Population
        {
            get { return citizen.Count; }
        }
    }

    internal class Person
    {
        private string name;

        public Person(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool Equals(Person other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.name, name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Person)) return false;
            return Equals((Person) obj);
        }

        public override int GetHashCode()
        {
            return (name != null ? name.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return string.Format("Name: {0}", name);
        }
    }
}