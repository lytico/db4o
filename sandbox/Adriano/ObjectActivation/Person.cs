namespace ObjectActivationSample
{
    public class Person : SupportObjectId
    {
        private Address address;

        public Address Address
        {
            get { return address; }
            set { address = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private int age;

        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        private Person parent;

        public Person Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public Person(string name, int age, Address address, Person parent)
        {
            this.name = name;
            this.address = address;
            this.age = age;
            this.parent = parent;
        }

        public override string ToString()
        {
            return PrintInfo(0);
        }

        private string PrintInfo(int i)
        {
            string tab = new string('\t', i);
            return
                string.Format("{0} {4} {1}{5}\r\n{4}{4}Address: {2}\r\n{4}{4}Parent: {3}",
                            (name ?? "null"),
                            age,
                            (address != null ? address.ToString() : "null"),
                            (parent != null ? parent.PrintInfo(i+1) : "null"),
                            tab,
                            base.ToString());
        }
    }
}
