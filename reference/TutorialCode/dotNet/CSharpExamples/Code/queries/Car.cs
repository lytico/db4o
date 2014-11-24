namespace Db4oTutorialCode.Code.Queries
{
    // #example: Domain model for cars
    public class Car
    {
        private readonly string carName;
        private readonly int horsePower;

        public Car(string carName, int horsePower)
        {
            this.carName = carName;
            this.horsePower = horsePower;
        }

        public string CarName
        {
            get { return carName; }
        }

        public int HorsePower
        {
            get { return horsePower; }
        }
    }
    // #end example
}