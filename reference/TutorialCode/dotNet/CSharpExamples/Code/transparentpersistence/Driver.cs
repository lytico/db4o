using System.Collections.Generic;

namespace Db4oTutorialCode.Code.TransparentPersistence
{
    // #example: Domain model for drivers
    [TransparentPersisted]
    public class Driver
    {
        private string name;
        private readonly IList<Car> ownedCars = new List<Car>();
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

        public IList<Car> OwnedCars
        {
            get { return ownedCars; }
        }

        public void AddOwnedCar(Car car)
        {
            ownedCars.Add(car);
        }
    }
    // #end example
}