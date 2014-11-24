namespace Db4oTutorialCode.Code.FirstSteps
{
    // #example: Domain model for drivers
    public class Driver
    {
        private string name;
        private Car mostLovedCar;

        public Driver(string name)
        {
            this.name = name;
        }

        public Driver(string name, Car mostLovedCar)
        {
            this.name = name;
            this.mostLovedCar = mostLovedCar;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Car MostLovedCar
        {
            get { return mostLovedCar; }
            set { mostLovedCar = value; }
        }
    }
    // #end example
}