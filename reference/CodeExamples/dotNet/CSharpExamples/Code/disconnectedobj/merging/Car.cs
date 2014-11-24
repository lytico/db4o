namespace Db4oDoc.Code.DisconnectedObj.Merging
{
    public class Car : IDHolder
    {
        private Pilot pilot;
        private string name;

        public Car(Pilot pilot, string name)
        {
            this.pilot = pilot;
            this.name = name;
        }

        public Pilot Pilot
        {
            get { return pilot; }
            set { pilot = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        public override string ToString()
        {
            return string.Format("{0} pilot: {1}", name, pilot);
        }
    }
}