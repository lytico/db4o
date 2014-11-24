namespace Db4oDoc.Code.Identiy
{
    
    class Pilot {
        private string name;

        public Pilot(string name) {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public override string ToString()
        {
            return string.Format("Name: {0}", name);
        }
    }

    class Car
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
            return string.Format("Pilot: {0}, Name: {1}", pilot, name);
        }
    }
}