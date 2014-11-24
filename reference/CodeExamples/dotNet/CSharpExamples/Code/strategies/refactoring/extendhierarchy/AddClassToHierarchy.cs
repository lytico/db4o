using System;
using System.Collections.Generic;
using Db4objects.Db4o;

namespace Db4oDoc.Code.Strategies.Refactoring.ExtendHierarchy
{
    public class AddClassToHierarchy
    {
        private const string DatabaseFileName = "database.db4o";

        public static void Main(string[] args)
        {
            StoreOldObjectLayout();
            ListItems();
            Console.WriteLine("--After refactoring--");
            CopyToNewType();
            ListItems();
        }

        private static void ListItems()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFileName))
            {
                IList<Mammal> allMammals = container.Query<Mammal>();
                foreach (Mammal mammal in allMammals)
                {
                    Console.WriteLine(mammal);
                }
            }
        }

        private static void CopyToNewType()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFileName))
            {
                // #example: copy the data from the old type to the new one
                IList<Human> allMammals = container.Query<Human>();
                foreach (Human oldHuman in allMammals)
                {
                    HumanNew newHuman = new HumanNew("");
                    newHuman.BodyTemperature = oldHuman.BodyTemperature;
                    newHuman.IQ = oldHuman.IQ;
                    newHuman.Name = oldHuman.Name;

                    container.Store(newHuman);
                    container.Delete(oldHuman);
                }
                // #end example
            }
        }

        private static void StoreOldObjectLayout()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFileName))
            {
                container.Store(new Mammal());
                container.Store(new Human("Joanna"));
                container.Store(new Human("Joanna"));
            }
        }
    }


    internal class Mammal
    {
        private int bodyTemperature;

        public int BodyTemperature
        {
            get { return bodyTemperature; }
            set { bodyTemperature = value; }
        }


        public override string ToString()
        {
            return "Mammal{" +
                   "bodyTemperature=" + bodyTemperature +
                   '}';
        }
    }

    internal class Primate : Mammal
    {
        private int iq;

        public int IQ
        {
            get { return iq; }
            set { iq = value; }
        }

        public override string ToString()
        {
            return "Primate{" +
                   "iq=" + iq +
                   "} is a " + base.ToString();
        }
    }

    internal class Human : Mammal
    {
        private string name;
        private int iq;

        public Human(string name)
        {
            this.name = name;
            BodyTemperature = 36;
            IQ = 120;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int IQ
        {
            get { return iq; }
            set { iq = value; }
        }


        public override string ToString()
        {
            return "Human{" +
                   "name='" + name + '\'' +
                   "} is a " + base.ToString();
        }
    }

    internal class HumanNew : Primate
    {
        private string name;


        public HumanNew(string name)
        {
            this.name = name;
            BodyTemperature = 36;
            IQ = 120;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public override string ToString()
        {
            return "Human{" +
                   "name='" + name + '\'' +
                   "} is a " + base.ToString();
        }
    }
}