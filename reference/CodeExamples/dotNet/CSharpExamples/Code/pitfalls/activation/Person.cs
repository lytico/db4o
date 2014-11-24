namespace Db4oDoc.Code.Pitfalls.Activation
{
    // #example: Person with a reference to the mother
    internal class Person
    {
        private Person mother;
        private string name;

        public Person(string name)
        {
            this.name = name;
        }

        public Person(Person mother, string name)
        {
            this.mother = mother;
            this.name = name;
        }

        public Person Mother
        {
            get { return mother; }
        }

        public string Name
        {
            get { return name; }
        }
    }
    // #end example
}