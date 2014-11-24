using System;
using System.Collections.Generic;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;

namespace Db4oDoc.Code.MetaInfo
{
    public class MetaInfoExample
    {
        public static void Main(string[] args)
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile("database.db4o"))
            {
                container.Store(new Person("Johnson", "Roman", 42));
                container.Store(new Person("Miller", "John", 21));

                // #example: All stored classes
                // Get the information about all stored classes.
                IStoredClass[] classesInDB = container.Ext().StoredClasses();
                foreach (IStoredClass storedClass in classesInDB)
                {
                    Console.WriteLine(storedClass.GetName());
                }

                // Information for a certain class
                IStoredClass metaInfo = container.Ext().StoredClass(typeof (Person));
                // #end example

                // #example: Accessing stored fields
                IStoredClass metaInfoForPerson = container.Ext().StoredClass(typeof (Person));
                // Access all existing fields
                foreach (IStoredField field in metaInfoForPerson.GetStoredFields())
                {
                    Console.WriteLine("Field: " + field.GetName());
                }
                // Accessing the field 'name' of any type.
                IStoredField nameField = metaInfoForPerson.StoredField("name", null);
                // Accessing the string field 'name'. Important if this field had another time in previous
                // versions of the class model
                IStoredField ageField = metaInfoForPerson.StoredField("age", typeof (int));

                // Check if the field is indexed
                bool isAgeFieldIndexed = ageField.HasIndex();

                // Get the type of the field
                string fieldType = ageField.GetStoredType().GetName();
                // #end example

                // #example: Access via meta data
                IStoredClass metaForPerson = container.Ext().StoredClass(typeof (Person));
                IStoredField metaNameField = metaForPerson.StoredField("name", null);

                IList<Person> persons = container.Query<Person>();
                foreach (Person person in persons)
                {
                    string name = (string) metaNameField.Get(person);
                    Console.WriteLine("Name is " + name);
                }
                // #end example
            }
        }
    }

    internal class Person
    {
        private string sirname;
        private string firstname;
        private int age;

        public Person(string sirname, string firstname, int age)
        {
            this.sirname = sirname;
            this.firstname = firstname;
            this.age = age;
        }
    }
}