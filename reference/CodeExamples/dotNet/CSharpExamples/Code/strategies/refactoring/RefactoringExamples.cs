using System;
using System.Collections.Generic;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;

namespace Db4oDoc.Code.Strategies.Refactoring
{
    public class RefactoringExamples
    {
        private const string DatabaseFile = "database.db4o";

        public static void Main(string[] args)
        {
            RenameFieldAndClass();
            ChangeType();
        }


        private static void ChangeType()
        {
            StoreInDB(new Person(), new Person("John"));


            // #example: copying the data from the old field type to the new one
            using (IObjectContainer container = Db4oEmbedded.OpenFile("database.db4o"))
            {
                // first get all objects which should be updated
                IList<Person> persons = container.Query<Person>();
                foreach (Person person in persons)
                {
                    // get the database-metadata about this object-type
                    IStoredClass dbClass = container.Ext().StoredClass(person);
                    // get the old field which was an int-type
                    IStoredField oldField = dbClass.StoredField("id", typeof (int));
                    if(null!=oldField)
                    {
                        // Access the old data and copy it to the new field!
                        object oldValue = oldField.Get(person);
                        if (null != oldValue)
                        {
                            person.id = new Identity((int)oldValue);
                            container.Store(person);
                        }
                    }
                }
            }

            // #end example
        }

        private static void RenameFieldAndClass()
        {
            CreateOldDatabase();


            IEmbeddedConfiguration configuration = RefactorClassAndFieldName();
            using (IObjectContainer container = Db4oEmbedded.OpenFile(configuration,DatabaseFile))
            {
                IList<PersonNew> persons = container.Query<PersonNew>();
                foreach (PersonNew person in persons)
                {
                    Console.Out.WriteLine(person.Sirname);
                }
            }
        }

        private static IEmbeddedConfiguration RefactorClassAndFieldName()
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            // #example: Rename a class
            configuration.Common.ObjectClass("Db4oDoc.Code.Strategies.Refactoring.PersonOld, Db4oDoc")
                .Rename("Db4oDoc.Code.Strategies.Refactoring.PersonNew, Db4oDoc");
            // #end example:
            // #example: Rename field
            configuration.Common.ObjectClass("Db4oDoc.Code.Strategies.Refactoring.PersonOld, Db4oDoc")
                .ObjectField("name").Rename("sirname");
            // #end example
            return configuration;
        }

        private static void CreateOldDatabase()
        {
            StoreInDB(new PersonOld(), new PersonOld("Papa Joe"));
        }

        private static void StoreInDB(params object[] objects)
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                foreach (object obj in objects)
                {
                    container.Store(obj);
                }
            }
        }
    }


    public class PersonOld
    {
        private string name = "Joe";

        public PersonOld()
        {
        }

        public PersonOld(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }

    public class PersonNew
    {
        private string sirname = "Joe";

        public string Sirname
        {
            get { return sirname; }
            set { sirname = value; }
        }
    }

    internal class Person
    {
        // #example: change type of field
        public Identity id = Identity.NewId();
        //  was an int previously:
        //    public int id = new Random().nextInt();
        // #end example

        private string name;

        public Person(String name)
        {
            this.name = name;
        }

        public Person() : this("Joe")
        {
        }

        public Identity ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        public override string ToString()
        {
            return "Person{" +
                   "id=" + id +
                   ", name='" + name + '\'' +
                   '}';
        }
    }

    internal class Identity
    {
        private readonly int id;

        public Identity(int id)
        {
            this.id = id;
        }

        public static Identity NewId()
        {
            return new Identity(new Random().Next());
        }

        public override string ToString()
        {
            return id.ToString();
        }
    }
}