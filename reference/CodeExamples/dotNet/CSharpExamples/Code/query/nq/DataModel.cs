namespace Db4oDoc.Code.Query.NativeQueries
{

    internal class Pilot
    {
        private string name;
        private int age;

        public Pilot(string name, int age)
        {
            this.name = name;
            this.age = age;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, Age: {1}", name, age);
        }
    }

    internal class Car
    {
        private Pilot pilot;
        private string name;


        public Car(Pilot pilot, string name)
        {
            this.pilot = pilot;
            this.name = name;
        }

        public Pilot Pilot
        {
            get { return pilot; }
            set { pilot = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public override string ToString()
        {
            return string.Format("Pilot: {0}, Name: {1}", pilot, name);
        }
    }
}