namespace Db4oDoc.Ta.Example
{
    public class Person
    {
        private string name;
        private Person mother;

        public Person(string name)
        {
            this.name = name;
        }

        public Person(string name, Person mother)
        {
            this.name = name;
            this.mother = mother;
        }

        public Person Mother
        {
            get
            {
                return mother;
            }
            set
            {
                mother = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }


        public override string ToString()
        {
            return name;
        }

        public static Person PersonWithHistory()
        {
            return CreateFamily(10);
        }

        private static Person CreateFamily(int generation)
        {
            if (0 < generation)
            {
                int previousGeneration = generation - 1;
                return new Person("Joanna the " + generation,
                                  CreateFamily(previousGeneration));
            }
            else
            {
                return null;
            }
        }
    }
}