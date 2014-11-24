using System;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;

namespace Db4oDoc.Code.Practises.Relations
{
    public class RelationManagementExamples
    {
        public static void Main(string[] args)
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile("database.db4o"))
            {
                StoreTestData(container);

                LoadPersonsOfACountry(container);
            }
        }

        private static void StoreTestData(IObjectContainer container)
        {
            var switzerland = new Country("Switzerland");
            var china = new Country("China");
            var japan = new Country("Japan");
            var usa = new Country("USA");
            var germany = new Country("Germany");

            container.Store(new Person("Berni", "Gian-Reto", switzerland));
            container.Store(new Person("Wang", "Long", china));
            container.Store(new Person("Tekashi", "Amuro", japan));
            container.Store(new Person("Miller", "John", usa));
            container.Store(new Person("Smith", "Paul", usa));
            container.Store(new Person("Müller", "Hans", germany));
        }

        private static void LoadPersonsOfACountry(IObjectContainer container)
        {
            // #example: Query for people burn in a country
            var country = LoadCountry(container, "USA");
            var peopleBurnInTheUs = from Person p in container
                                    where p.BornIn == country
                                    select p;
            // #end example
            Console.Out.WriteLine(peopleBurnInTheUs.Count());
        }

        private static Country LoadCountry(IObjectContainer container, string countryName)
        {
            return (from Country c in container
                    where c.Name == countryName
                    select c).Single();
        }
    }
}