namespace Db4oTutorialCode.Code.FirstSteps
{

    // #example: Domain model for cars
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