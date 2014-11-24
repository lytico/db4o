using Db4objects.Db4o.Config.Attributes;

namespace Db4oTutorialCode.Code.Indexes
{
    // #example: Indexing the car name
    public class Car
    {
        [Indexed] private string carName;
        private int horsePower;

        public Car(string carName, int horsePower)
        {
            this.carName = carName;
            this.horsePower = horsePower;
        }

        public string CarName
        {
            get { return carName; }
            set { carName = value; }
        }

        public int HorsePower
        {
            get { return horsePower; }
            set { horsePower = value; }
        }
    }
    // #end example
}