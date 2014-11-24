namespace Db4oDoc.Code.DisconnectedObj.IdExamples
{
    public class Pilot : IDHolder
    {
        private string name;

        public Pilot(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public override string ToString()
        {
            return name;
        }
    }
}