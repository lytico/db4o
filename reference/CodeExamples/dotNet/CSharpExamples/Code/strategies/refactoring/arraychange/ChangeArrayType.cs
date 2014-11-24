using System;
using System.Collections.Generic;
using System.IO;
using Db4objects.Db4o;

namespace Db4oDoc.Code.Strategies.Refactoring.ArrayChange
{
    public class ChangeArrayType
    {
        public const string DatabaseFile = "database.db4o";

        public static void Main(string[] args)
        {
            CleanUp();
            StoreOldData();
            ListItems<PersonOld>();
            RefactorToArrayType();
            ListItems<PersonNew>();
        }

        private static void RefactorToArrayType()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #example: Copy the string-field to the new string-array field
                IList<PersonOld> oldPersons = container.Query<PersonOld>();
                foreach (PersonOld old in oldPersons)
                {
                    PersonNew newPerson = new PersonNew();
                    newPerson.Name = new string[] {old.Name};
                    container.Store(newPerson);
                    container.Delete(old);
                }
                // #end example
            }
        }

        private static void ListItems<T>()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                foreach (object obj in container.Query<T>())
                {
                    Console.WriteLine(obj);
                }
            }
        }

        private static void StoreOldData()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                container.Store(new PersonOld("Joe"));
                container.Store(new PersonOld("Joanna"));
                container.Store(new PersonOld("Joel"));
            }
        }


        private static void CleanUp()
        {
            File.Delete(DatabaseFile);
        }
    }


    internal class PersonOld
    {
        private string name = "Roman";

        public PersonOld(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public override string ToString()
        {
            return string.Format("Name: {0}", name);
        }
    }

    internal class PersonNew
    {
        private string[] name = new string[] {"Roman"};

        public PersonNew(params string[] name)
        {
            this.name = name;
        }

        public string[] Name
        {
            get { return name; }
            set { name = value; }
        }

        public override string ToString()
        {
            return string.Format("Name: {0}", name);
        }
    }
}