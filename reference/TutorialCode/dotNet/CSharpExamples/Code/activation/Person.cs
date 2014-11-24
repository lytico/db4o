namespace Db4oTutorialCode.Code.Activation
{
    // #example: Domain model for people
    public class Person
    {
        private readonly string name;
        private readonly Person mother;

        public Person(string name, Person mother)
        {
            this.name = name;
            this.mother = mother;
        }

        public string Name
        {
            get { return name; }
        }

        public Person Mother
        {
            get { return mother; }
        }

        public override string ToString()
        {
            return string.Format("Name: {0}", name);
        }
    }

// #end example
}