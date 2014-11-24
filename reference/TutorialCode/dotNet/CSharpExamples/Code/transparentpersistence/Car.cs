namespace Db4oTutorialCode.Code.TransparentPersistence
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
            set { carName = value; }
        }
    }
    // #end example
}