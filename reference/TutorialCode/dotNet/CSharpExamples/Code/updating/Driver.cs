using System.Collections.Generic;

namespace Db4oTutorialCode.Code.Updating
{
    // #example: Domain model for drivers
    public class Driver
    {
        private string name;
        private Car mostLovedCar;
        private IList<Car> ownedCars = new List<Car>();

        public Driver(string name)
        {
            this.name = name;
        }

        public Driver(string name,Car mostLovedCar)
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

        public IList<Car> OwnedCars
        {
            get { return ownedCars; }
            set { ownedCars = value; }
        }

        public void AddOwnedCar(Car item)
        {
            ownedCars.Add(item);
        }
    }
    // #end example
}