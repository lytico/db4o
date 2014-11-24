namespace Db4oDoc.Code.DisconnectedObj.Merging
{
    public class Pilot : IDHolder
    {
        private string name;
        private int points;

        public Pilot(string name)
        {
            this.name = name;
        }

        public Pilot(string name, int points)
        {
            this.name = name;
            this.points = points;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Points
        {
            get { return points; }
            set { points = value; }
        }

        public override string ToString()
        {
            return string.Format("{0} with {1}",name,points);
        }
    }
}