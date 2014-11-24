using Db4oTutorialCode.Code.TransparentPersistence;

namespace Db4oTutorialCode.Code.Transactions
{
    // #example: Domain model for drivers
    [TransparentPersisted]
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
        }
    }
    // #end example
}