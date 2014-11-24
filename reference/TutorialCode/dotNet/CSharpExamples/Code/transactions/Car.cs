using Db4oTutorialCode.Code.TransparentPersistence;

namespace Db4oTutorialCode.Code.Transactions
{
    // #example: Domain model for cars
    [TransparentPersisted]
    public class Car
    {
        private string carName;

        public Car(string carName)
        {
            this.carName = carName;
        }

        public string CarName
        {
            get { return carName; }
        }
    }
    // #end example
}