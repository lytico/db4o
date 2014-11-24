namespace Db4oTutorialCode.Code.Queries
{
    // #example: Domain model for drivers
    public class Driver
    {
        private string name;
        private int age;
        private Car mostLovedCar;

        public Driver(string name, int age)
        {
            this.name = name;
            this.age = age;
        }

        public Driver(string name,int age, Car mostLovedCar)
        {
            this.name = name;
            this.age = age;
            this.mostLovedCar = mostLovedCar;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Age
        {
            get { return age; }
        }

        public Car MostLovedCar
        {
            get { return mostLovedCar; }
            set { mostLovedCar = value; }
        }
    }
    // #end example
}